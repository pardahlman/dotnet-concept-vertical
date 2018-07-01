using Concept.Vertical.Messaging.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RawRabbit;
using RawRabbit.Instantiation;

namespace Concept.Vertical.Messaging.RabbitMQ
{
  public static class UseRabbitMqHostExtension
  {
    public static IWebHostBuilder UseRabbitMq(this IWebHostBuilder builder)
    {
      builder.ConfigureServices(collection => collection.AddInMemoryServices());
      return builder;
    }

    public static IHostBuilder UseRabbitMq(this IHostBuilder builder)
    {
      builder.ConfigureServices((context, collection) => collection.AddInMemoryServices());
      return builder;
    }

    private static IServiceCollection AddInMemoryServices(this IServiceCollection collection)
    {
      collection
        .AddSingleton<IBusClient>(RawRabbitFactory.CreateSingleton())
        .AddSingleton<IMessagePublisher, MessagePublisher>()
        .AddSingleton<IMessageSubscriber, MessageSubscriber>();
      return collection;
    }
  }
}
