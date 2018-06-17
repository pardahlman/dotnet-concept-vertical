using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Concept.Vertical.ReadComponent;
using Concept.Vertical.ReadComponent.Domain;
using Concept.Vertical.SpaComponent;
using Concept.Vertical.Tests.Framework;
using Xunit;

namespace Concept.Vertical.Tests
{
  public class UnitTest1 : IClassFixture<WebhostFixture<SpaStartup>>
  {
    private readonly WebhostFixture<SpaStartup> _webhostComponent;

    public UnitTest1(WebhostFixture<SpaStartup> webhostComponent)
    {
      _webhostComponent = webhostComponent;
    }

    [Fact]
    public async Task Should_Post_Stop_Command_When_Clicking_Stop()
    {
      var stopTask =  _webhostComponent.Intercept<ToggleWeatherUpdates>();

      var stopRecieved = new TaskCompletionSource<ToggleWeatherUpdates>();
      await _webhostComponent.Intercept<ToggleWeatherUpdates>(stop => stopRecieved.TrySetResult(stop));

      await _webhostComponent.Intercept<ToggleWeatherUpdates, WeatherStopped>((stopCommand, token) => Task.FromResult(new WeatherStopped()));

      // TODO: Setup Selenium

      await stopTask;

      Assert.NotNull(stopRecieved.Task.Result);
    }

    [Fact]
    public async Task Should_Update_List_Of_Forecasts_On_Event()
    {
      while (true)
      {
        await Task.Delay(TimeSpan.FromSeconds(10));

        await _webhostComponent.PushAsync(new WeatherUpdated
        {
          Forecasts = new List<WeatherUpdateService.WeatherForecast>
          {
            new WeatherUpdateService.WeatherForecast
            {
              DateFormatted = "2020-01-01",
              Summary = "Blazing cold",
              TemperatureC = -38
            }
          }
        });

      }

      await Task.Delay(TimeSpan.FromHours(1));
    }
  }

  public class WeatherStopped { }
}
