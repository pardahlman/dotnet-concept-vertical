using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

namespace Concept.Vertical.Web.SignalR
{
  internal class ApplicationHub : Hub
  {
    private readonly IMessageForwarder _forwarder;

    public ApplicationHub(IMessageForwarder forwarder)
    {
      _forwarder = forwarder;
    }

    public Task PublishData(JObject message, string exchangeName, string routingKey)
    {
      return _forwarder.PublishAsync(message, exchangeName, routingKey);
    }
  }
}
