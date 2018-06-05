using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Vertical.Tests.Framework
{
  public class WebhostFixture<TStartup> : IDisposable, IServiceProvider where TStartup : class
  {
    private readonly IWebHost _webHost;
    public Uri ServerUri { get; }

    public WebhostFixture()
    {
      ServerUri = ResolveUniqueUnusedUrl();

      _webHost = WebHost
        .CreateDefaultBuilder<TStartup>(new string[0])
        .UseUrls(ServerUri.ToString())
        .ConfigureServices(RegisterTestSpecificServices)
        .Build();
    }

    // TODO: Register test specific services
    private static void RegisterTestSpecificServices(IServiceCollection collenction) { }

    // TODO: Select port based on something
    private Uri ResolveUniqueUnusedUrl() => new Uri("http://localhost:5003", UriKind.Absolute);

    public void Dispose()
    {
      _webHost.Dispose();
    }

    public object GetService(Type serviceType) => _webHost.Services.GetService(serviceType);
  }
}
