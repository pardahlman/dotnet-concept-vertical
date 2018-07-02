using System;
using System.Collections.Generic;

namespace Concept.Vertical.Web.Bootstrap
{
  public class SpaBootstrap
  {
    public string DomElement { get; set; }
    public Uri BaseUrl { get; set; }
    public IEnumerable<Uri> ScriptResources { get; set; }
  }
}
