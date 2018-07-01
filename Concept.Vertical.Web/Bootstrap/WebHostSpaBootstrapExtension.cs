using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Vertical.Web.Bootstrap
{
  public static class WebHostSpaBootstrapExtension
  {
    public static IWebHostBuilder RenderSpaComponent(this IWebHostBuilder builder, params SpaBootstrap[] bootstrap)
    {
      builder
        .ConfigureServices(collection =>
        {
          foreach (var spaBootstrap in bootstrap)
          {
            collection.AddSingleton(spaBootstrap);
          }
        });
      return builder;
    }

    public static IWebHostBuilder RenderSpaComponent(this IWebHostBuilder builder, string domElement, params Uri[] resourceUris) 
      => builder.RenderSpaComponent(new SpaBootstrap {DomElement = domElement, ResourceUris = resourceUris});
  }
}
