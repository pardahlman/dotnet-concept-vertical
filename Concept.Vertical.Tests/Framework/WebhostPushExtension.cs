using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Messaging;
using Concept.Vertical.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Vertical.Tests.Framework
{
  public static class WebhostPushExtension
  {
    public static async Task PushAsync<TMessage>(this IWebhostFixture fixture, TMessage message, CancellationToken ct = default)
    {
      var publisher = fixture.GetService<IMessagePublisher>();
      await publisher.PublishAsync(new ClientMessage
      {
        Type = message.GetType().Name,
        Payload = message,
        ClientIds = new List<string>()
      }, ct);
    }
  }
}
