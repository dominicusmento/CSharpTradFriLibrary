using System.Net.WebSockets;
using System.Text;
using ApiLibs.General;
using Newtonsoft.Json;
using Tomidix.NetStandard.Dirigera.Model.Events;

namespace Tomidix.NetStandard.Dirigera.Controller;

public class EventController : SubService<DirigeraController>
{
    public class DirigeraEventArgs : EventArgs
    {
        public DirigeraEventArgs(string message)
        {
            Message = message;
        }

        public DirigeraEventArgs() { }

        public required string Message { get; set; }
        public required DirigeraEvent Event { get; set; }
    }

    public EventController(DirigeraController controller) : base(controller)
    {

    }

    public event EventHandler<DirigeraEventArgs>? OnEventSent;

    private ClientWebSocket ws;


    // Wrap event invocations inside a protected virtual method
    // to allow derived classes to override the event invocation behavior
    protected virtual void OnRaiseDirigeraEvent(DirigeraEventArgs e)
    {
        // Make a temporary copy of the event to avoid possibility of
        // a race condition if the last subscriber unsubscribes
        // immediately after the null check and before the event is raised.
        EventHandler<DirigeraEventArgs>? raiseEvent = OnEventSent;

        // Event will be null if there are no subscribers
        if (raiseEvent != null)
        {
            // Format the string to send inside the CustomEventArgs parameter
            e.Message += $" at {DateTime.Now}";

            // Call to raise the event.
            raiseEvent(this, e);
        }
    }

    public async Task Connect(CancellationToken cancellationToken)
    {
        this.ws = new();
        ws.Options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        ws.Options.SetRequestHeader("Authorization", "Bearer " + Service.token);

        await ws.ConnectAsync(new Uri($"wss://{Service.hostUrl}:8443/v1"), cancellationToken);
        new Task(async () =>
        {

            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
                try
                {
                    var result = await ws.ReceiveAsync(buffer, cancellationToken);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    var message = Encoding.UTF8.GetString(buffer.Array ?? [], 0, result.Count);

                    // The message is the json file with ` at TIMESTAMP` text appended for some weird reason
                    var split = message.Split(" at ");
                    OnRaiseDirigeraEvent(new DirigeraEventArgs
                    {
                        Message = message,
                        Event = JsonConvert.DeserializeObject<DirigeraEvent>(split[0])
                    });
                } 
                catch (TaskCanceledException)
                {
                    break;
                }
                
            }
        }, cancellationToken).Start();


    }

    public void SendKeepAliveMessages(CancellationToken cancellationToken) {
        var timer = new System.Timers.Timer(30000);
        // Hook up the Elapsed event for the timer. 
        timer.Elapsed += async (obj, e) => {
            if(cancellationToken.IsCancellationRequested)    {
                timer.Stop();
                timer.Dispose();
                return;
            }

            try {
                await SendPingMessage();            
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

        };
        timer.AutoReset = true;
        timer.Enabled = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="https://github.com/lpgera/dirigera/blob/main/src/ws.ts#L39"/>
    /// <returns></returns>
    private async Task SendPingMessage()
    {
        await SendMessage(new DirigeraEvent
        {
            Id = Guid.NewGuid().ToString(),
            Specversion = "1.1.0",
            Source = "urn:lpgera:dirigera",
            Time = DateTimeOffset.Now,
            Type = "ping",
        });
    }

    private Task SendMessage(DirigeraEvent data, CancellationToken? cancellationToken = null)
    {
        return SendMessage(JsonConvert.SerializeObject(data), cancellationToken);
    }

    private Task SendMessage(string data, CancellationToken? cancellationToken = null)
    {
        var encoded = Encoding.UTF8.GetBytes(data);
        var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);
        return ws.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken ?? CancellationToken.None);
    }

}
