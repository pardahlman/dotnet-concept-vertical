using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Concept.Vertical.Messaging;
using Concept.Vertical.ReadComponent;
using Concept.Vertical.ReadComponent.Domain;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        .Build();

    public static IWebHost CreateApplicationHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .ConfigureServices(collection =>
        {
          collection.AddSingleton<IMessagePublisher>(InMemoryMessageBus.Instance);
          collection.AddSingleton<IMessageSubscriber>(InMemoryMessageBus.Instance);
          collection.AddSingleton<ITypeIdentifierMap>(new TypeIdentifierMap(new Dictionary<string, Type> { { nameof(StopCommand), typeof(StopCommand) } }));
        })
        .UseStartup<Web.Startup>()
        .UseUrls("http://localhost:5001")
        .UseContentRoot(@"d:\Code\Github\dotnet-concept-vertical\Concept.Vertical.Web")
        .RegisterLogicalComponent<WeatherUpdateService>()
        .Build();

    public static IHost CreateReadHost(string[] args) =>
      new HostBuilder()
        .ConfigureServices(collection =>
        {
          collection.AddSingleton<IMessagePublisher>(InMemoryMessageBus.Instance);
          collection.AddSingleton<IMessageSubscriber>(InMemoryMessageBus.Instance);
          collection.AddSingleton<IHostedService, WeatherUpdateService>();
        })
        .Build();
  }
}
