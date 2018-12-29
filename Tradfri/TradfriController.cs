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

namespace Tradfri
{
    public class TradfriController : Service
    {
        private CoapClient CoapClient;

        public DeviceController DeviceController { get; set; }
        public GatewayController GatewayController { get; set; }
        public GroupController GroupController { get; set; }
        public SmartTaskController SmartTasks { get; set; }

        public string GateWayName { get; }

        public TradfriController(string GateWayName, string gatewayIp) : base(gatewayIp)
        {
            this.GateWayName = GateWayName;

            DeviceController = new DeviceController(this);
            GatewayController = new GatewayController(this);
            GroupController = new GroupController(this);
            
        }

        public void Connect(string GatewaySecret)
        {
            var auth = GeneratePSK(GatewaySecret, GateWayName);
            OneKey authKey = new OneKey();
            authKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            authKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(auth.PSK)));
            authKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes(GateWayName)));

            DTLSClientEndPoint ep = new DTLSClientEndPoint(authKey);
            CoapClient cc = new CoapClient(new Uri($"coaps://{Client.BaseUrl}"))
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
            request.UriPath = Client.BaseUrl + url;

            if (content != null)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                request.SetPayload(JsonConvert.SerializeObject(content, settings));
            }

            Task<Response> t = new Task<Response>(() => {
                return CoapClient.Send(request);
            });

            Response resp = await t;

            if((int)resp.StatusCode != (int) statusCode)
            {
                throw new Exception(resp.ResponseText);
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
            r.SetUri($"coaps://{Client.BaseUrl}" + $"/{(int)TradfriConstRoot.Gateway}/{(int)TradfriConstAttr.Auth}/");
            r.EndPoint = ep;
            r.AckTimeout = 5000;
            r.SetPayload($@"{{""{(int)TradfriConstAttr.Identity}"":""{applicationName}""}}");

            r.Send();
            resp = r.WaitForResponse(5000);

            return Convert<TradfriAuth>(resp.PayloadString);
        }
    }
}
    