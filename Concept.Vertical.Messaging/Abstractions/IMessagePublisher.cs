using System.Threading;
using System.Threading.Tasks;

namespace Concept.Vertical.Messaging.Abstractions
{
  public interface IMessagePublisher
  {
    Task PublishAsync<TMessage>(TMessage message, CancellationToken token);
  }
}
