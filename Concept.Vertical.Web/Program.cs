using Concept.Vertical.Messaging.RabbitMQ;
using Concept.Vertical.Web.SignalR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using RawRabbit.Serialization;

namespace Concept.Vertical.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
      return WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseRabbitMq(ioc => ioc.AddSingleton<ISerializer>(
            new JsonSerializer(new Newtonsoft.Json.JsonSerializer
            {
              Converters = {new ClientMessageConverter()}
            }
          )));
    }
  }
}
