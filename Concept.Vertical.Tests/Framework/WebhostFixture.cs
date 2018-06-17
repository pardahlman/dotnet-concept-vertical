using System;
using Concept.Vertical.Messaging.Abstractions;
using Concept.Vertical.Messaging.InMemory;
using Concept.Vertical.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Concept.Vertical.Tests.Framework
{
  public interface IWebhostFixture : IServiceProvider { }

  public class WebhostFixture<TStartup> : IDisposable, IWebhostFixture where TStartup : class
  {
    private readonly IWebHost _webHost;
    private readonly IWebHost _spaHost;

    public Uri VerticalUri { get; }

    public WebhostFixture()
    {
      VerticalUri = ResolveUniqueUnusedUrl();

      _webHost = WebHost
        .CreateDefaultBuilder<Startup>(new string[0])
        .UseProjectContentRoot<Startup>()
        .UseUrls("http://localhost:5001")
        .ConfigureServices(RegisterTestSpecificContaingWebServices)
        .Build();

      _spaHost = WebHost
        .CreateDefaultBuilder<TStartup>(new string[0])
        .UseProjectContentRoot<TStartup>()
        .UseUrls(VerticalUri.ToString())
        .ConfigureServices(RegisterTestSpecificServices)
        .Build();

      _spaHost.Start();
      _webHost.Start();
    }

    private static void RegisterTestSpecificServices(IServiceCollection collection)
    {
      collection
        //.AddSingleton(new JsonSerializer { Converters = { new ClientMessageConverter() }, ContractResolver = new CamelCasePropertyNamesContractResolver() })
        .AddSingleton<JsonSerializer>()
        .AddSingleton<IMessagePublisher, MessagePublisher>()
        .AddSingleton<IMessageSubscriber, MessageSubscriber>();
    }

    private static void RegisterTestSpecificContaingWebServices(IServiceCollection collection)
    {
      collection
        .AddSingleton(new JsonSerializer { Converters = { new ClientMessageConverter() }, ContractResolver = new CamelCasePropertyNamesContractResolver() })
        .AddSingleton<IMessagePublisher, MessagePublisher>()
        .AddSingleton<IMessageSubscriber, MessageSubscriber>();
    }

    // TODO: Select port based on something
    private Uri ResolveUniqueUnusedUrl() => new Uri("http://localhost:5000", UriKind.Absolute);

    public void Dispose()
    {
      _webHost.Dispose();
      _spaHost.Dispose();
    }

    public object GetService(Type serviceType) => _spaHost.Services.GetService(serviceType);
  }
}
