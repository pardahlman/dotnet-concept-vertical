using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Messaging;
using Concept.Vertical.Messaging.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace Concept.Vertical.Web.SignalR
{
  internal class HostedSubscriber : IHostedService
  {
    private readonly IMessageSubscriber _subscriber;
    private readonly IHubContext<ApplicationHub> _hubContext;

    public HostedSubscriber(IMessageSubscriber subscriber, IHubContext<ApplicationHub> hubContext)
    {
      _subscriber = subscriber;
      _hubContext = hubContext;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      await _subscriber.SubscribeAsync<ClientMessage>(async (message, ct) =>
      {
        var clients = message.ClientIds.Any()
          ? _hubContext.Clients.Clients(message.ClientIds)
          : _hubContext.Clients.All;
        await clients.SendAsync("dataReceived", message, ct);
      }, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }
  }
}
