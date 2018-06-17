using Concept.Vertical.Messaging.InMemory;
using Concept.Vertical.Web.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Concept.Vertical.Web
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddSignalR();
      services.AddSingleton<IHostedService, HostedSubscriber>();
      services.AddSingleton<IConnection>(new Connection());
      services.AddSingleton<IMessageForwarder, MessageForwarder>();
      services.AddCors(options => options
        .AddPolicy("LocalSpa", cors => cors
          .AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowCredentials()
          .AllowAnyMethod()));
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseDeveloperExceptionPage();
      app.UseStaticFiles();
      app.UseMvc();
      app.UseCors("LocalSpa");
      app.UseSignalR(route => route.MapHub<ApplicationHub>("/application"));
    }
  }
}
