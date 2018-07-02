using Concept.Vertical.Hosting;
using Concept.Vertical.Messaging.RabbitMQ;
using Example.Todo.ListComponent;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Example.Todo.ListView
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseLogicalComponent<ListLogicalComponent>()
        .UseRabbitMq();
  }
}
