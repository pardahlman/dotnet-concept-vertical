using System.Text;
using Concept.Vertical.Messaging.InMemory;
using Newtonsoft.Json.Linq;
using RawRabbit;
using RawRabbit.Configuration.BasicPublish;

namespace Concept.Vertical.Web.SignalR
{
  public interface IMessageForwarder
  {
    void Publish(JObject body, string exchange, string routingKey);
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

    public void Publish(JObject body, string exchange, string routingKey)
    {
      _channel.BasicPublish(routingKey, new BasicProperties(), Encoding.UTF8.GetBytes(body.ToString()));
    }
  }

  public class RabbitMqMessageForwarder : IMessageForwarder
  {
    private readonly IBusClient _busClient;

    public RabbitMqMessageForwarder(IBusClient busClient)
    {
      _busClient = busClient;
    }

    public void Publish(JObject body, string exchange, string routingKey)
    {
      _busClient.BasicPublishAsync(new BasicPublishConfiguration
      {
        RoutingKey = routingKey,
        BasicProperties = new RabbitMQ.Client.Framing.BasicProperties(),
        Body = Encoding.UTF8.GetBytes(body.ToString()),
        ExchangeName = exchange,
        Mandatory = false
      });
    }
  }
}
