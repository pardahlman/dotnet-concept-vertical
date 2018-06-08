using System;
using System.Collections.Generic;
using Concept.Vertical.ReadComponent.Domain;

namespace Concept.Vertical.Tests.Framework
{
  public interface ITypeIdentifierMap
  {
    bool TryGetType(string identifier, out Type type);
  }

  public class TypeIdentifierMap : ITypeIdentifierMap
  {
    // TODO: Use attribute mapping
    private static readonly IDictionary<string, Type> _knownTypes = new Dictionary<string, Type>
    {
      {nameof(StopCommand), typeof(StopCommand)}
    };

    public bool TryGetType(string identifier, out Type type) => _knownTypes.TryGetValue(identifier, out type);
  }
}
