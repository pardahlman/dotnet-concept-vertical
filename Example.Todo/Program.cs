using System.Threading.Tasks;
using Concept.Vertical.Hosting;
using Concept.Vertical.Messaging.InMemory;
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
      var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

      var writeHost = new HostBuilder()
        .RegisterLogicalComponent<TodoLogicalComponent>(c => c.AddTodoServcies())
        .UseInMemoryMessaging()
        .Build();

      var listConfig = config.GetSection("todoList").Get<WebComponentConfiguration>();
      var listHost = WebHost
        .CreateDefaultBuilder<ListView.Startup>(args)
        .UseUrls(listConfig.BaseUrl.ToString())
        .UseInMemoryMessaging()
        .RegisterLogicalComponent<ListLogicalComponent>()
        .Build();

      var webFrameworkHost = WebHost
        .CreateDefaultBuilder<Concept.Vertical.Web.Startup>(args)
        .UseInMemoryMessaging()
        .RenderSpaComponent(listConfig.DomElement, listConfig.GetScriptUris())
        .Build();

      await Task.WhenAll(writeHost.StartAsync(), listHost.StartAsync());
      await webFrameworkHost.RunAsync();
      await Task.WhenAll(writeHost.StopAsync(), listHost.StopAsync());
    }
  }
}
