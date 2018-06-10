using System;
using Concept.Vertical.Messaging;
using Concept.Vertical.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

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

      _spaHost = WebHost
        .CreateDefaultBuilder<TStartup>(new string[0])
        .UseProjectContentRoot<TStartup>()
        .UseUrls(VerticalUri.ToString())
        .ConfigureServices(RegisterTestSpecificServices)
        .Build();

      _webHost = WebHost
        .CreateDefaultBuilder<Startup>(new string[0])
        .UseUrls("http://localhost:5001")
        .ConfigureServices(RegisterTestSpecificServices)
        .Build();

      _webHost.Start();
      _spaHost.Start();
    }

    private static void RegisterTestSpecificServices(IServiceCollection collection)
    {
      collection
        .AddSingleton<IMessagePublisher>(InMemoryMessageBus.Instance)
        .AddSingleton<IMessageSubscriber>(InMemoryMessageBus.Instance);
    }

    // TODO: Select port based on something
    private Uri ResolveUniqueUnusedUrl() => new Uri("http://localhost:5000", UriKind.Absolute);

    public void Dispose()
    {
      _webHost.Dispose();
      _spaHost.Dispose();
    }

    public object GetService(Type serviceType) => _webHost.Services.GetService(serviceType);
  }
}
