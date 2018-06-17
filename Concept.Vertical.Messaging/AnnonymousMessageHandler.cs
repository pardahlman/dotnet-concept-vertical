  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using Concept.Vertical.Messaging.Abstractions;

namespace Concept.Vertical.Messaging
{
  public static class AnnonymousMessageHandlerExtension
  {
    public static Task SubscribeAsync<TMessage>(
      this IMessageSubscriber subscriber,
      Func<TMessage, CancellationToken, Task> handlerFunc,
      CancellationToken token = default)
    {
      return subscriber.SubscribeAsync(new AnnonymousMessageHandler<TMessage>(handlerFunc), token);
    }

    private class AnnonymousMessageHandler<TMessage> : IMessageHandler<TMessage>
    {
      private readonly Func<TMessage, CancellationToken, Task> _handlerFunc;

      public AnnonymousMessageHandler(Func<TMessage, CancellationToken, Task> handlerFunc)
      {
        _handlerFunc = handlerFunc;
      }

      public Task HandleAsync(TMessage message, CancellationToken token) => _handlerFunc(message, token);
    }
  }
}
