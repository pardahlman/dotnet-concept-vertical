using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Vertical.Tests.Framework
{
  public static class WebhostPublishExtension
  {
    public static async Task PublishAsync<TMessage>(this IWebhostFixture fixture, TMessage message, CancellationToken ct = default)
    {
      var publisher = fixture.GetService<IMessagePublisher>();
      await publisher.PublishAsync(message, ct);
    }

  }
}
