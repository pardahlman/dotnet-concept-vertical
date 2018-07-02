using System;
using System.Collections.Generic;
using System.Linq;
using Concept.Vertical.Web.Bootstrap;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Concept.Vertical.Web.Pages
{
  public class IndexModel : PageModel
  {
    public IReadOnlyCollection<string> ComponentDomIds { get; private set; }
    public IReadOnlyCollection<Uri> ComponentScriptSrc { get; private set; }

    public IndexModel(IEnumerable<SpaBootstrap> spaBootstraps)
    {
      spaBootstraps = spaBootstraps.ToList();
      ComponentDomIds = spaBootstraps.Select(b => b.DomElement).ToList().AsReadOnly();
      ComponentScriptSrc = spaBootstraps
        .SelectMany(b => b.ScriptResources
          .Select(uri => uri.IsAbsoluteUri ? uri : new Uri(b.BaseUrl, uri.OriginalString)))
        .ToList()
        .AsReadOnly();
    }
  }
}