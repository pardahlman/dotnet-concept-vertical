using Concept.Vertical.Messaging.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Concept.Vertical.Messaging.InMemory
{
  public static class UseInMemoryMessagingExtension
  {
    private static readonly JsonSerializer Serializer = new JsonSerializer();

    public static IWebHostBuilder UseInMemoryMessaging(this IWebHostBuilder builder)
    {
      builder.ConfigureServices(collection => collection.AddInMemoryServices());
      return builder;
    }

    public static IHostBuilder UseInMemoryMessaging(this IHostBuilder builder)
    {
      builder.ConfigureServices((context, collection) => collection.AddInMemoryServices());
      return builder;
    }

    private static IServiceCollection AddInMemoryServices(this IServiceCollection collection)
    {
      collection
        .AddSingleton<IMessagePublisher>(new MessagePublisher(Serializer))
        .AddSingleton<IMessageSubscriber>(new MessageSubscriber(Serializer));
      return collection;
    }
  }
}
