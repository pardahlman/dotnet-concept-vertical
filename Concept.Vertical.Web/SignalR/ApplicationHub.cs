using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Messaging;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

namespace Concept.Vertical.Web.SignalR
{
  internal class ApplicationHub : Hub
  {
    private readonly IMessagePublisher _publisher;

    public ApplicationHub(IMessagePublisher publisher)
    {
      _publisher = publisher;
    }

    public async Task PublishData(JObject message, string messageTypeIdentifier)
    {
      await _publisher.PublishAsync(new UnparsedMessage
      {
        Payload = ConvertJsonToMessageBusSerialization(message),
        PayloadType = messageTypeIdentifier
      }, CancellationToken.None);
    }

    // TODO: formalize this, do nothing for now
    private static object ConvertJsonToMessageBusSerialization(JObject message)
    {
      return message;
    }
  }
}
