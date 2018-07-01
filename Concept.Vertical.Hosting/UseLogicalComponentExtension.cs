using System;
using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Abstractions;
using Concept.Vertical.Messaging.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace Concept.Vertical.Hosting
{
  public static class UseLogicalComponentExtension
  {
    private class DisposableHostedService<THosted> : IHostedService, IDisposable where THosted : ILogicalComponent
    {
      private readonly IServiceCollection _componentServices;
      private ServiceProvider _provider;
      private THosted _hostedService;

      public DisposableHostedService(IServiceCollection componentServices)
      {
        _componentServices = componentServices;
      }

      public Task StartAsync(CancellationToken cancellationToken)
      {
        _provider = _componentServices.BuildServiceProvider();
        _hostedService = _provider.GetRequiredService<THosted>();
        return _hostedService.StartAsync(cancellationToken);
      }

      public Task StopAsync(CancellationToken cancellationToken)
        => _hostedService.StopAsync(cancellationToken);

      public void Dispose()
      {
        _provider.Dispose();
      }
    }

    public static IHostBuilder RegisterLogicalComponent<TLogicalComponent>(this IHostBuilder builder,
      Action<IServiceCollection> componentServices = null)
      where TLogicalComponent : class, ILogicalComponent
    {
      builder
        .ConfigureServices((context, collection) => collection
          .AddSingleton<IHostedService>(applicationProvider =>
          {
            var serviceCollection = new ServiceCollection()
              .AddSingleton<TLogicalComponent>()
              .AddSingleton(applicationProvider.GetService<IMessagePublisher>())
              .AddSingleton(applicationProvider.GetService<IMessageSubscriber>());
            componentServices?.Invoke(serviceCollection);
            return new DisposableHostedService<TLogicalComponent>(serviceCollection);
          }));
      return builder;
    }

    public static IWebHostBuilder RegisterLogicalComponent<TLogicalComponent>(this IWebHostBuilder builder, Action<IServiceCollection> componentServices = null)
      where TLogicalComponent : class, ILogicalComponent
    {
      builder
        .ConfigureServices(applicationCollection => applicationCollection
          .AddSingleton<IHostedService>(applicationProvider =>
          {
            var serviceCollection = new ServiceCollection()
              .AddSingleton<TLogicalComponent>()
              .AddSingleton(applicationProvider.GetService<IMessagePublisher>())
              .AddSingleton(applicationProvider.GetService<IMessageSubscriber>())
              .AddSingleton(applicationProvider.GetService<IApplicationLifetime>());
            componentServices?.Invoke(serviceCollection);
            return new DisposableHostedService<TLogicalComponent>(serviceCollection);
          }));
      return builder;
    }
  }
}