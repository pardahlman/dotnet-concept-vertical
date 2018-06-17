using System.Collections.Generic;

namespace Concept.Vertical.ReadComponent.Domain
{
  public class WeatherUpdated
  {
    public List<WeatherUpdateService.WeatherForecast> Forecasts { get; set; }
  }
}
