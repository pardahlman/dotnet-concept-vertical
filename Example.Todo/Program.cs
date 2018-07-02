using System.Threading.Tasks;
using Concept.Vertical.Hosting;
using Concept.Vertical.Messaging.InMemory;
using Concept.Vertical.Messaging.RabbitMQ;
using Concept.Vertical.Web.Bootstrap;
using Example.Todo.BusinessComponent;
using Example.Todo.ListComponent;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace Example.Todo
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var writeHost = new HostBuilder()
        .UseLogicalComponent<TodoLogicalComponent>(c => c.AddTodoServcies())
        .UseRabbitMq()
        .Build();

      await writeHost.RunAsync();
    }
  }
}
