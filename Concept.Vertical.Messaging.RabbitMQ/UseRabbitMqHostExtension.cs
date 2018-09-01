using System;
using Concept.Vertical.Messaging.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RawRabbit;
using RawRabbit.DependencyInjection;
using RawRabbit.Instantiation;
using RawRabbit.Serialization;

namespace Concept.Vertical.Messaging.RabbitMQ
{
  public static class UseRabbitMqHostExtension
  {
    public static IWebHostBuilder UseRabbitMq(this IWebHostBuilder builder, Action<IDependencyRegister> ioc = null)
    {
      builder.ConfigureServices(collection => collection.AddRawRabbitServices(ioc));
      return builder;
    }

    public static IHostBuilder UseRabbitMq(this IHostBuilder builder, Action<IDependencyRegister> ioc = null)
    {
      builder.ConfigureServices((context, collection) => collection.AddRawRabbitServices(ioc));
      return builder;
    }

    private static IServiceCollection AddRawRabbitServices(this IServiceCollection collection,
      Action<IDependencyRegister> ioc)
    {
      Action<IDependencyRegister> customServices = register =>
        register.AddSingleton<ISerializer>(new JsonSerializer(new Newtonsoft.Json.JsonSerializer()));

      if (ioc != null)
      {
        customServices += ioc;
      }

      collection
        .AddSingleton<IBusClient>(RawRabbitFactory.CreateSingleton(new RawRabbitOptions
        {
          DependencyInjection = customServices
        }))
        .AddSingleton<IMessagePublisher, MessagePublisher>()
        .AddSingleton<IMessageSubscriber, MessageSubscriber>();
      return collection;
    }
  }
}
