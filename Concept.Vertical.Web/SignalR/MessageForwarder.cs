using System.Text;
using Concept.Vertical.Messaging.InMemory;
using Newtonsoft.Json.Linq;

namespace Concept.Vertical.Web.SignalR
{
  public interface IMessageForwarder
  {
    void Publish(JObject payload, string routingKey);
  }

  public class MessageForwarder : IMessageForwarder
  {
    private readonly IConnection _brokerConnection;
    private readonly IModel _channel;

    public MessageForwarder(IConnection brokerConnection)
    {
      _brokerConnection = brokerConnection;
      _channel = brokerConnection.CreateModel();
    }

    public void Publish(JObject payload, string routingKey)
    {
      _channel.BasicPublish(routingKey, new BasicProperties(), Encoding.UTF8.GetBytes(payload.ToString()));
    }
  }
}
