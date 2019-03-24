using ApiLibs;
using ApiLibs.General;
using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.DTLS;
using Com.AugustCellars.COSE;
using Newtonsoft.Json;
using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tomidix.NetStandard.Tradfri.Controllers;
using Tomidix.NetStandard.Tradfri.Models;
using Method = Com.AugustCellars.CoAP.Method;

namespace Tomidix.NetStandard.Tradfri
{
    public class TradfriController : Service
    {
        private CoapClient _coapClient;

        public DeviceController DeviceController { get; set; }
        public GatewayController GatewayController { get; set; }
        public GroupController GroupController { get; set; }
        public SmartTaskController SmartTasksController { get; set; }
        private readonly string _gatewayIp;

        public string GateWayName { get; }

        public TradfriController(string gatewayName, string gatewayIp) : base("https://www.ikea.com/") //Ignore this
        {
            this.GateWayName = gatewayName;
            _gatewayIp = gatewayIp;

            DeviceController = new DeviceController(this);
            GatewayController = new GatewayController(this);
            GroupController = new GroupController(this);
            SmartTasksController = new SmartTaskController(this);
        }
        [Obsolete("This is an old way of connecting to Gateway. You should use with two parameters, then generate AppKey and use it in. Usefull for Unit Testing.", false)]
        public TradfriController(string gatewayName, string gatewayIp, string PSK) : this(gatewayName, gatewayIp)
        {
            ConnectPSK(PSK);
        }

        public void ConnectPSK(string GatewaySecret)
        {
            OneKey authKey = new OneKey();
            authKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            authKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(GatewaySecret)));

            DTLSClientEndPoint ep = new DTLSClientEndPoint(authKey);
            CoapClient cc = new CoapClient(new Uri($"coaps://{_gatewayIp}"))
            {
                EndPoint = ep
            };

            ep.Start();

            _coapClient = cc;
        }

        public void ConnectAppKey(string appKey, string applicationName)
        {
            OneKey authKey = new OneKey();
            authKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            authKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(appKey)));
            authKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes(applicationName)));

            DTLSClientEndPoint ep = new DTLSClientEndPoint(authKey);
            CoapClient cc = new CoapClient(new Uri($"coaps://{_gatewayIp}"))
            {
                EndPoint = ep
            };

            ep.Start();

            _coapClient = cc;
        }

        //This is the interface of the entire library. Every request that is made outside of this class will use this method to communicate.
        protected override async Task<string> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Request request = new Request(ConvertToMethod(call));
            request.UriPath = url;

            // this is done on purpose to handle ObserveDevice from DeviceController
            if (statusCode.Equals(HttpStatusCode.Continue))
            {
                request.MarkObserve();
                request.Respond += (Object sender, ResponseEventArgs e) =>
                {
                    ((Action<Response>)content).Invoke(request.Response);
                };
            }

            if (content != null && !statusCode.Equals(HttpStatusCode.Continue))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                request.SetPayload(JsonConvert.SerializeObject(content, settings));
            }

            Task<Response> t = new Task<Response>(() =>
            {
                return _coapClient.Send(request);
            });

            t.Start();

            Response resp = await t;

            if (MapToHttpStatusCode(resp.StatusCode) != (int)statusCode)
            {
                RequestException<Response>.ConvertToException(MapToHttpStatusCode(resp.StatusCode), resp.StatusCode.ToString(), resp.UriQuery, "", resp.ResponseText, resp);
            }

            return resp.ResponseText;
        }

        private Method ConvertToMethod(Call call)
        {
            switch (call)
            {
                case Call.GET:
                    return Method.GET;
                case Call.POST:
                    return Method.POST;
                case Call.DELETE:
                    return Method.DELETE;
                case Call.PUT:
                    return Method.PUT;
                default:
                    throw new NotSupportedException("This is not supported for coap");
            }
        }

        /// <summary>
        /// Acquire All Resources
        /// </summary>
        /// <returns></returns>
        public List<WebLink> GetResources()
        {
            return _coapClient.Discover().ToList();
        }

        public TradfriAuth GenerateAppSecret(string GatewaySecret, string applicationName)
        {
            Response resp = new Response(StatusCode.Valid);

            OneKey authKey = new OneKey();
            authKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            authKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(GatewaySecret)));
            authKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("Client_identity")));
            DTLSClientEndPoint ep = new DTLSClientEndPoint(authKey);
            ep.Start();

            Request r = new Request(Method.POST);
            r.SetUri($"coaps://{_gatewayIp}" + $"/{(int)TradfriConstRoot.Gateway}/{(int)TradfriConstAttr.Auth}/");
            r.EndPoint = ep;
            r.AckTimeout = 5000;
            r.SetPayload($@"{{""{(int)TradfriConstAttr.Identity}"":""{applicationName}""}}");

            r.Send();
            resp = r.WaitForResponse();

            if ((int)resp.StatusCode != 201)
            {
                RequestException<Response>.ConvertToException(MapToHttpStatusCode(resp.StatusCode), resp.StatusCode.ToString(), resp.UriQuery, "", resp.ResponseText, resp);
            }

            return Convert<TradfriAuth>(resp.PayloadString);
        }

        private int MapToHttpStatusCode(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Created:
                    return 201;
                case StatusCode.Deleted:
                    return 204;
                case StatusCode.Valid:
                    return 200;
                case StatusCode.Changed:
                    return 200;
                case StatusCode.Content:
                    return 200;

                case StatusCode.BadRequest:
                    return 400;
                case StatusCode.BadOption:
                    return 400;
                case StatusCode.Forbidden:
                    return 403;
                case StatusCode.NotFound:
                    return 404;
                case StatusCode.MethodNotAllowed:
                    return 405;
                case StatusCode.NotAcceptable:
                    return 406;
                case StatusCode.PreconditionFailed:
                    return 412;
                case StatusCode.RequestEntityTooLarge:
                    return 413;
                case StatusCode.UnsupportedMediaType:
                    return 414;

                case StatusCode.InternalServerError:
                    return 500;
                case StatusCode.NotImplemented:
                    return 501;
                case StatusCode.ServiceUnavailable:
                    return 503;
                case StatusCode.GatewayTimeout:
                    return 504;
                case StatusCode.ProxyingNotSupported:
                    return 500;

                default:
                    return 400;


            }
        }
    }
}
