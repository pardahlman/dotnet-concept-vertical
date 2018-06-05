using System.Threading;
using System.Threading.Tasks;

namespace Concept.Vertical.Messaging
{
  public interface IMessagePublisher
  {
    Task PublishAsync<TMessage>(TMessage message, CancellationToken token);
  }
}
