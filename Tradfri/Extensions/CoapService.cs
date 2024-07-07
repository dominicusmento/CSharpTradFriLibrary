using System;
using System.Threading.Tasks;
using ApiLibs;
using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.DTLS;
using Newtonsoft.Json;

namespace Tomidix.NetStandard.Tradfri.Extensions
{
    public class CoapImplementation : ICallImplementation
    {
        internal CoapClient _coapClient;


        public override async Task<RequestResponse> ExecuteRequest(Service service2, ApiLibs.Request request)
        {
            var coapRequest = new Com.AugustCellars.CoAP.Request(ConvertToMethod(request.Method))
            {
                UriPath = request.EndPoint
            };

            if (request.Content != null)
            {
                coapRequest.SetPayload(AddBody(request.Content));
            }

            // this is done on purpose to handle ObserveDevice from DeviceController
            if (request is WatchRequest watch)
            {
                coapRequest.MarkObserve();
                Action cancelWatch = () => coapRequest.MarkObserveCancel();
                coapRequest.Respond += (object sender, ResponseEventArgs e) =>
                {
                    watch.EventHandler?.Invoke(coapRequest.Response, cancelWatch);
                };
            }

            if(request is EndPointRequest<object> a)
            {
                coapRequest.EndPoint = a.DTLSEndPoint;
            }

            Task<Response> requestTask = new Task<Response>(() =>
            {
                return _coapClient.Send(coapRequest);
            });

            requestTask.Start();

            Response resp = await requestTask;

            if(resp == null)
            {
                throw new Exception("Request timed out");
            }

            var responseApiLibs = new RequestResponse((System.Net.HttpStatusCode)MapToHttpStatusCode(resp.StatusCode), resp.StatusCode.ToString(), resp.UriQuery, "", resp.ResponseText, resp, request, service2);

            if (resp.IsTimedOut)
            {
                throw new RequestTimeoutResponse(responseApiLibs).ToException();
            }

            return responseApiLibs;
        }

        private static string AddBody(object content)
        {
            return content switch
            {
                string text => text,
                var randomObject => JsonConvert.SerializeObject(randomObject, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                })
            };
        }

        private int MapToHttpStatusCode(StatusCode statusCode) =>
            statusCode switch
            {
                StatusCode.Created => 201,
                StatusCode.Deleted => 200,
                StatusCode.Valid => 200,
                StatusCode.Changed => 200,
                StatusCode.Content => 200,
                StatusCode.BadRequest => 400,
                StatusCode.BadOption => 400,
                StatusCode.Forbidden => 403,
                StatusCode.NotFound => 404,
                StatusCode.MethodNotAllowed => 405,
                StatusCode.NotAcceptable => 406,
                StatusCode.PreconditionFailed => 412,
                StatusCode.RequestEntityTooLarge => 413,
                StatusCode.UnsupportedMediaType => 414,
                StatusCode.InternalServerError => 500,
                StatusCode.NotImplemented => 501,
                StatusCode.ServiceUnavailable => 503,
                StatusCode.GatewayTimeout => 504,
                StatusCode.ProxyingNotSupported => 500,
                _ => 400,
            };

        private Method ConvertToMethod(Call call)
        {
            return call switch
            {
                Call.GET => Method.GET,
                Call.POST => Method.POST,
                Call.DELETE => Method.DELETE,
                Call.PUT => Method.PUT,
                _ => throw new System.NotSupportedException("This is not supported for coap"),
            };
        }
    }

    public class WatchRequest : ApiLibs.Request
    {
        public WatchRequest(string endPoint) : base(endPoint)
        {
        }

        public Action<Response, Action> EventHandler { get; internal set; }
    }

    public class EndPointRequest<T> : Request<T>
    {
        public EndPointRequest(string endPoint) : base(endPoint)
        {
        }

        public DTLSClientEndPoint DTLSEndPoint { get; set; }
    }
}