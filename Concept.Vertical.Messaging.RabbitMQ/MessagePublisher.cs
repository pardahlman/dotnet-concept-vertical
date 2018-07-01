using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Messaging.Abstractions;
using RawRabbit;

namespace Concept.Vertical.Messaging.RabbitMQ
{
  public class MessagePublisher : IMessagePublisher
  {
    private readonly IBusClient _busClient;

    public MessagePublisher(IBusClient busClient)
    {
      _busClient = busClient;
    }

    public Task PublishAsync<TMessage>(TMessage message, CancellationToken token)
      => _busClient.PublishAsync(message, token: token);
  }
}
