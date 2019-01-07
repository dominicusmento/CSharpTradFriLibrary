using ApiLibs;
using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.DTLS;
using Com.AugustCellars.COSE;
using Newtonsoft.Json;
using PeterO.Cbor;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tradfri.Models;
using Tradfri.Controllers;
using Method = Com.AugustCellars.CoAP.Method;
using System.Linq;
using ApiLibs.General;

namespace Tradfri
{
    public class TradfriController : Service
    {
        private CoapClient CoapClient;

        public DeviceController DeviceController { get; set; }
        public GatewayController GatewayController { get; set; }
        public GroupController GroupController { get; set; }
        public SmartTaskController SmartTasksController { get; set; }
        private readonly string _gatewayIp;

        public string GateWayName { get; }

        public TradfriController(string GateWayName, string gatewayIp) : base("https://www.ikea.com/") //Ignore this
        {
            this.GateWayName = GateWayName;

            DeviceController = new DeviceController(this);
            GatewayController = new GatewayController(this);
            GroupController = new GroupController(this);
            SmartTasksController = new SmartTaskController(this);
            _gatewayIp = gatewayIp;
        }

        public TradfriController(string GateWayName, string gatewayIp, string PSK) : this(GateWayName, gatewayIp)
        {
            OneKey authKey = new OneKey();
            authKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            authKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(PSK)));
            authKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes(GateWayName)));

            DTLSClientEndPoint ep = new DTLSClientEndPoint(authKey);
            CoapClient cc = new CoapClient(new Uri($"coaps://{_gatewayIp}"))
            {
                EndPoint = ep
            };

            ep.Start();

            CoapClient = cc;
            CoapClient.Timeout = 500;
        }

        public void Connect(string GatewaySecret)
        {
            var auth = GeneratePSK(GatewaySecret, GateWayName);
            OneKey authKey = new OneKey();
            authKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            authKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(auth.PSK)));
            authKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes(GateWayName)));

            DTLSClientEndPoint ep = new DTLSClientEndPoint(authKey);
            CoapClient cc = new CoapClient(new Uri($"coaps://{_gatewayIp}"))
            {
                EndPoint = ep
            };

            ep.Start();

            CoapClient = cc;
        }

        //This is the interface of the entire library. Every request that is made outside of this class will use this method to communicate.
        protected override async Task<string> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Request request = new Request(ConvertToMethod(call));
            request.UriPath = url;

            if (content != null)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                request.SetPayload(JsonConvert.SerializeObject(content, settings));
            }

            Task<Response> t = new Task<Response>(() => {
                Response response = CoapClient.Send(request);
                return response;
            });

            t.Start();

            Response resp = await t;

            if (MapToHttpStatusCode(resp.StatusCode) != (int) statusCode)
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
            return CoapClient.Discover().ToList();
        }

        public TradfriAuth GeneratePSK(string GatewaySecret, string applicationName)
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
            switch(statusCode)
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
    