using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Abstractions;
using Concept.Vertical.Messaging;
using Concept.Vertical.Messaging.Abstractions;
using Concept.Vertical.ReadComponent.Domain;

namespace Concept.Vertical.ReadComponent
{
  public class WeatherUpdateService : ILogicalComponent
  {
    private readonly IMessagePublisher _publisher;
    private readonly IMessageSubscriber _subscriber;
    private bool active = true;

    private static readonly string[] Summaries = {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private CancellationTokenSource _cancellationSource;

    public WeatherUpdateService(IMessagePublisher publisher, IMessageSubscriber subscriber)
    {
      _publisher = publisher;
      _subscriber = subscriber;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      _cancellationSource = new CancellationTokenSource();

      await _subscriber.SubscribeAsync<ToggleWeatherUpdates>((updates, token) =>
      {
        active = !active;
        return Task.CompletedTask;
      }, _cancellationSource.Token);

      await Task.Run(async () =>
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
          }).ToList();

          if (active)
          {
            await _publisher.PublishAsync(new ClientMessage
            {
              Payload = new WeatherUpdated { Forecasts = newForecast },
              Type = nameof(WeatherUpdated),
              ClientIds = new string[0]
            }, _cancellationSource.Token);
          }
        }
      }, cancellationToken);
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
