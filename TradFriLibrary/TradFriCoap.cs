using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.DTLS;
using Com.AugustCellars.COSE;
using PeterO.Cbor;
using System;
using System.Text;

namespace Tomidix.CSharpTradFriLibrary
{
    public class TradFriCoapConnector
    {
        public CoapClient Client { get; set; }
        public string Identity { get; }
        public string PreSharedKey { get; }
        public string GatewayIp { get; }
        public TradFriCoapConnector(string id, string ip, string key)
        {
            Identity = id; GatewayIp = ip; PreSharedKey = key;
        }

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
            Client = cc;
        }
    }

    public static class TradFriCommunicator
    {
        #region Extension Methods
        public static Response SetValues(this CoapClient _client, TradFriRequest request)
        {
            _client.UriPath = request.UriPath;
            return _client.Put(request.Payload);
        }
        public static Response GetValues(this CoapClient _client, TradFriRequest request)
        {
            _client.UriPath = request.UriPath;
            return _client.Get();
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
