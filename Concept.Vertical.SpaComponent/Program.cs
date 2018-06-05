using System;
using System.IO;
using System.Threading.Tasks;
using Concept.Vertical.Messaging;
using Concept.Vertical.ReadComponent;
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
      var readHost = CreateReadHost(args);
      await Task.WhenAll(
        spaHost.StartAsync(),
        applicationHost.StartAsync(),
        readHost.StartAsync()
      );

      await Task.Delay(TimeSpan.FromHours(1));
    }

    public static IWebHost CreateSpaHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseUrls("http://localhost:5000")
        .Build();

    public static IWebHost CreateApplicationHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .ConfigureServices(collection =>
        {
          collection.AddSingleton<IMessagePublisher>(InMemoryMessageBus.Instance);
          collection.AddSingleton<IMessageSubscriber>(InMemoryMessageBus.Instance);
        })
        .UseStartup<Web.Startup>()
        .UseUrls("http://localhost:5001")
        .UseContentRoot(@"D:\Code\Github\dotnet-concept-vertical\Concept.Vertical.Web")
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
