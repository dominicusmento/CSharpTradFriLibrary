using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.DTLS;
using Com.AugustCellars.COSE;
using Newtonsoft.Json;
using PeterO.Cbor;
using System;
using System.Text;
using Tomidix.CSharpTradFriLibrary.Controllers;
using Tomidix.CSharpTradFriLibrary.Models;

namespace Tomidix.CSharpTradFriLibrary
{
    public class TradFriCoapConnector
    {
        public CoapClient Client { get; set; }
        public string Identity { get; }
        public string PreSharedKey { get; }
        public string GatewayIp { get; }
        public GatewayInfo GatewayInfo { get; private set; }

        [Obsolete("This is an old way of using TradFriCoapConnector. You should use ConnectAppKey() method and constructor with two parameters.", false)]
        public TradFriCoapConnector(string id, string ip, string key)
        {
            Identity = id; GatewayIp = ip; PreSharedKey = key;
        }

        [Obsolete("This is an old way of connecting the client. You should use ConnectAppKey() method which obtains specific key for your application.", false)]
        public void Connect()
        {
            OneKey userKey = new OneKey();
            userKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            userKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(PreSharedKey)));

            DTLSClientEndPoint ep = new DTLSClientEndPoint(userKey);

            CoapClient cc = new CoapClient(new Uri($"coaps://{GatewayIp}"))
            {
                EndPoint = ep
            };

            ep.Start();

            GatewayController gc = new GatewayController(cc);
            GatewayInfo = gc.GetGatewayInfo();
            Client = cc;
        }

        public TradFriCoapConnector(string id, string ip)
        {
            Identity = id; GatewayIp = ip;
        }
        public void ConnectAppKey(string psk, string applicationName)
        {
            OneKey authKey = new OneKey();
            authKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            authKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(psk)));
            authKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes(applicationName)));

            DTLSClientEndPoint ep = new DTLSClientEndPoint(authKey);
            CoapClient cc = new CoapClient(new Uri($"coaps://{GatewayIp}"))
            {
                EndPoint = ep
            };

            ep.Start();

            GatewayController gc = new GatewayController(cc);
            GatewayInfo = gc.GetGatewayInfo();
            Client = cc;
        }

        public TradFriAuth GeneratePSK(string GatewaySecret, string applicationName)
        {
            Response resp = new Response(StatusCode.Valid);

            try
            {
                OneKey authKey = new OneKey();
                authKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
                authKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(GatewaySecret)));
                authKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("Client_identity")));
                DTLSClientEndPoint ep = new DTLSClientEndPoint(authKey);
                ep.Start();

                Request r = new Request(Method.POST);
                r.SetUri($"coaps://{GatewayIp}" + $"/{(int)TradFriConstRoot.Gateway}/{(int)TradFriConstAttr.Auth}/");
                r.EndPoint = ep;
                r.AckTimeout = 5000;
                r.SetPayload($@"{{""{(int)TradFriConstAttr.Identity}"":""{applicationName}""}}");

                r.Send();
                resp = r.WaitForResponse(5000);

                return JsonConvert.DeserializeObject<TradFriAuth>(resp.PayloadString);
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);

                var content = JsonConvert.DeserializeObject<dynamic>(resp.PayloadString);
                return new TradFriAuth();
            }
        }

    }



    public static class TradFriCommunicator
    {
        #region Extension Methods
        public static Response SetValues(this CoapClient _client, TradFriRequest request)
        {
            _client.UriPath = request.UriPath;
            return _client.Post(request.Payload);
        }

        public static Response GetValues(this CoapClient _client, TradFriRequest request)
        {
            _client.UriPath = request.UriPath;
            return _client.Get();
        }

        public static Response UpdateValues(this CoapClient _client, TradFriRequest request)
        {
            _client.UriPath = request.UriPath;
            return _client.Put(request.Payload);
        }

        public static CoapObserveRelation Observe(this CoapClient _client, TradFriRequest request, Action<Response> callback, Action<CoapClient.FailReason> error = null)
        {
            _client.UriPath = request.UriPath;
            return _client.Observe(callback, error);
        }

        [Obsolete("Method AcquireID() is too risky cause it extracts device id from its url string. You should use GetDevices() method which will already return the list of their IDs.")]
        public static long AcquireID(this WebLink _link)
        {
            try
            {
                return long.Parse(_link.Uri.Substring(_link.Uri.LastIndexOf('/') + 1));
            }
            catch
            {
                return -1;
            }
        }
        #endregion
    }

    public class TradFriRequest
    {
        public string UriPath { get; set; }
        public string Payload { get; set; }
    }
}
