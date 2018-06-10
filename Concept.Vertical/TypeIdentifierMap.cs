using System;
using System.Collections.Generic;

namespace Concept.Vertical
{
  public interface ITypeIdentifierMap
  {
    bool TryGetType(string identifier, out Type type);
  }

  public class TypeIdentifierMap : ITypeIdentifierMap
  {
    private readonly IDictionary<string, Type> _knownTypes;

    public TypeIdentifierMap(IDictionary<string,Type> knownTypes)
    {
      _knownTypes = knownTypes;
    }

    public bool TryGetType(string identifier, out Type type) => _knownTypes.TryGetValue(identifier, out type);
  }
}
