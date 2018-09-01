using System.Collections.Generic;
using Concept.Vertical.Messaging.InMemory;
using Concept.Vertical.Web.Bootstrap;
using Concept.Vertical.Web.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Concept.Vertical.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services
        .AddSignalR()
        .AddJsonProtocol(o => o.PayloadSerializerSettings = new JsonSerializerSettings
        {
          ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
      services.AddSingleton<IHostedService, HostedSubscriber>();
      services.AddSingleton<IConnection>(new Connection());
      services.AddSingleton<IMessageForwarder, RabbitMqMessageForwarder>();
      services.AddSingleton<IEnumerable<SpaBootstrap>>(Configuration.GetSection("components").Get<SpaBootstrap[]>());
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
