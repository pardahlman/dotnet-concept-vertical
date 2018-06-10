using System;
using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Vertical.Tests.Framework
{
  public static class WebhostInterceptExtension
  {
    public static Task<TMessage> Intercept<TMessage>(this IWebhostFixture fixture)
    {
      var messageCompletionSource = new TaskCompletionSource<TMessage>();
      fixture.Intercept<TMessage>(message => messageCompletionSource.TrySetResult(message));
      return messageCompletionSource.Task;
    }

    public static Task Intercept<TMessage>(this IWebhostFixture fixture, Action<TMessage> interceptor)
    {
      return fixture.Intercept<TMessage>((message, token) =>
      {
        interceptor(message);
        return Task.CompletedTask;
      });
    }

    public static Task Intercept<TMessage>(this IWebhostFixture fixture, Func<TMessage, CancellationToken, Task> interceptor)
    {
      return fixture
        .GetService<IMessageSubscriber>()
        .SubscribeAsync<ClientMessage>((message, token) =>
        {
          if (message.Payload is TMessage typedMessage)
          {
            return interceptor(typedMessage, token);
          }

          return Task.CompletedTask;
        });
    }

    public static Task Intercept<TIncomming, TOutgoing>(this IWebhostFixture fixture, Func<TIncomming, CancellationToken, Task<TOutgoing>> interceptor)
    {
      var publisher = fixture.GetService<IMessagePublisher>();
      return fixture.Intercept<TIncomming>(async (incomming, token) =>
      {
        var outgoing = await interceptor(incomming, token);
        await publisher.PublishAsync(outgoing, token);
      });
    }
  }
}
