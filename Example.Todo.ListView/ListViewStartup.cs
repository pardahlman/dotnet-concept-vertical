using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Todo.ListView
{
  public class ListViewStartup
  {
    public ListViewStartup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "ClientApp/build";
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseDeveloperExceptionPage();
      app.UseStaticFiles();
      app.UseSpaStaticFiles();
      app.UseMvc();
      app.UseSpa(spa =>
      {
        spa.Options.SourcePath = $"{spa.ApplicationBuilder.ApplicationServices.GetService<IHostingEnvironment>().ContentRootPath}\\ClientApp";
        spa.UseReactDevelopmentServer(npmScript: "start");
      });
    }
  }
}
