using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Messaging.Abstractions;
using RawRabbit;

namespace Concept.Vertical.Messaging.RabbitMQ
{
  public class MessageSubscriber : IMessageSubscriber
  {
    private readonly IBusClient _busClient;

    public MessageSubscriber(IBusClient busClient)
    {
      _busClient = busClient;
    }

    public Task SubscribeAsync<TMessage>(IMessageHandler<TMessage> handler, CancellationToken token)
      => _busClient.SubscribeAsync<TMessage>(message => handler.HandleAsync(message, token), ct: token);
  }
}
