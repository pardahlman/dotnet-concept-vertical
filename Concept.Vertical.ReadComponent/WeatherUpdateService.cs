using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Messaging;
using Microsoft.Extensions.Hosting;

namespace Concept.Vertical.ReadComponent
{
  public class WeatherUpdateService : IHostedService
  {
    private readonly IMessagePublisher _publisher;

    private static readonly string[] Summaries = {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private CancellationTokenSource _cancellationSource;

    public WeatherUpdateService(IMessagePublisher publisher)
    {
      _publisher = publisher;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _cancellationSource = new CancellationTokenSource();
      Task.Run(async () =>
      {
        while (true)
        {
          await Task.Delay(TimeSpan.FromSeconds(1), _cancellationSource.Token);
          var rng = new Random();
          var newForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
          {
            DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
          });

          await _publisher.PublishAsync(new ClientMessage
          {
            Payload = newForecast,
            RoutingKey = nameof(WeatherForecast),
            ClientIds = new string[0]
          }, _cancellationSource.Token);
        }
      }, cancellationToken);
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _cancellationSource.Cancel();
      return Task.CompletedTask;
    }

    public class WeatherForecast
    {
      public string DateFormatted { get; set; }
      public int TemperatureC { get; set; }
      public string Summary { get; set; }

      public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
  }
}
