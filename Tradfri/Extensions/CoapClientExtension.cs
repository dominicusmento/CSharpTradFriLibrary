using Com.AugustCellars.CoAP;
using System;
using Tomidix.NetStandard.Tradfri.Models;

namespace Tomidix.NetStandard.Tradfri.Extensions
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
