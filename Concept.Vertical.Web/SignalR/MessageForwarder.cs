using System.Text;
using System.Threading.Tasks;
using Concept.Vertical.Messaging.InMemory;
using Newtonsoft.Json.Linq;
using RawRabbit;
using RawRabbit.Configuration.BasicPublish;

namespace Concept.Vertical.Web.SignalR
{
  public interface IMessageForwarder
  {
    Task PublishAsync(JObject body, string exchange, string routingKey);
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

    public Task PublishAsync(JObject body, string exchange, string routingKey)
    {
      _channel.BasicPublish(routingKey, new BasicProperties(), Encoding.UTF8.GetBytes(body.ToString()));
      return Task.CompletedTask;
    }
  }

  public class RabbitMqMessageForwarder : IMessageForwarder
  {
    private readonly IBusClient _busClient;

    public RabbitMqMessageForwarder(IBusClient busClient)
    {
      _busClient = busClient;
    }

    public async Task PublishAsync(JObject body, string exchange, string routingKey)
    {
      await _busClient.BasicPublishAsync(new BasicPublishConfiguration
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
