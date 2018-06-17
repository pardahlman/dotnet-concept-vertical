using System;
using System.Threading.Tasks;
using Concept.Vertical.Hosting;
using Concept.Vertical.Messaging.Abstractions;
using Concept.Vertical.Messaging.InMemory;
using Concept.Vertical.ReadComponent;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Concept.Vertical.SpaComponent
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var spaHost = CreateSpaHost(args);
      var applicationHost = CreateApplicationHost(args);
      await Task.WhenAll(
        spaHost.StartAsync(),
        applicationHost.StartAsync()
      );

      await Task.Delay(TimeSpan.FromHours(1));
    }

    public static IWebHost CreateSpaHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<SpaStartup>()
        .UseUrls("http://localhost:5000")
        .ConfigureServices(collection =>
        {
          collection.AddSingleton<IMessageSubscriber, MessageSubscriber>();
          collection.AddSingleton<IMessagePublisher, MessagePublisher>();
          collection.AddSingleton<JsonSerializer>();
        })
        .RegisterLogicalComponent<WeatherUpdateService>()
        .Build();

    public static IWebHost CreateApplicationHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .ConfigureServices(collection =>
        {
          collection.AddSingleton<IMessageSubscriber, MessageSubscriber>();
          collection.AddSingleton(new JsonSerializer{Converters = { new ClientMessageConverter()}, ContractResolver = new CamelCasePropertyNamesContractResolver()});
        })
        .UseStartup<Web.Startup>()
        .UseUrls("http://localhost:5001")
        .UseContentRoot(@"d:\Code\Github\dotnet-concept-vertical\Concept.Vertical.Web")
        .Build();
  }
}
