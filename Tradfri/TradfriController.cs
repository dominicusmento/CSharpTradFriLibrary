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
using Tomidix.NetStandard.Tradfri.Extensions;
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

        public TradfriController(string gatewayName, string gatewayIp) : base(new CoapImplementation()) //Implementation of the COAP model
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

            (Implementation as CoapImplementation)._coapClient = new CoapClient(new Uri($"coaps://{_gatewayIp}"))
            {
                EndPoint = ep
            };

            ep.Start();
        }

        public void ConnectAppKey(string appKey, string applicationName)
        {
            OneKey authKey = new OneKey();
            authKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            authKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(appKey)));
            authKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes(applicationName)));

            DTLSClientEndPoint ep = new DTLSClientEndPoint(authKey);
            (Implementation as CoapImplementation)._coapClient = new CoapClient(new Uri($"coaps://{_gatewayIp}"))
            {
                EndPoint = ep
            };

            ep.Start();
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

        public Task<TradfriAuth> GenerateAppSecret(string GatewaySecret, string applicationName)
        {
            OneKey authKey = new OneKey();
            authKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            authKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(GatewaySecret)));
            authKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("Client_identity")));

            var ep = new DTLSClientEndPoint(authKey);
            ep.Start();

            return MakeRequest(new EndPointRequest<TradfriAuth>($"/{(int)TradfriConstRoot.Gateway}/{(int)TradfriConstAttr.Auth}/")
            {
                Content = $"{{\"{(int)TradfriConstAttr.Identity}\":\"{applicationName}\"}}",
                Method = Call.POST,
                DTLSEndPoint = ep,
                RequestHandler = (resp) => resp switch
                {
                    CreatedResponse crea => crea.Convert<TradfriAuth>(),
                    var other => throw other.ToException()
                }
            });
        }
    }
}
