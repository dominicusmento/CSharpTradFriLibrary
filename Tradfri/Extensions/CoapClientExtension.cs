using Com.AugustCellars.CoAP;
using System;
using Tradfri.Models;

namespace Tradfri.Extensions
{
    public static class CoapClientExtension
    {
        public static CoapObserveRelation Observe(this CoapClient _client, string url, Action<Response> callback, Action<CoapClient.FailReason> error = null)
        {
            _client.UriPath = url;
            return _client.Observe(callback, error);
        }
    }
}
