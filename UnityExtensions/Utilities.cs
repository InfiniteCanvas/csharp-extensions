using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using JetBrains.Annotations;

namespace Common.UnityExtensions;

public static class Utilities
{
    private static readonly Dictionary<int, object> _objects = new();

    public static TSource Provide<TSource>([NotNull] this TSource instance, bool replace = false) where TSource : new()
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));

        int hash = typeof(TSource).GetHashCode();
        if (replace && _objects.ContainsKey(hash)) _objects[hash] = instance;
        else _objects.Add(hash, instance);

        return instance;
    }

    public static TSource Retrieve<TSource>() where TSource : new()
    {
        int hash = typeof(TSource).GetHashCode();
        if (_objects.TryGetValue(hash, out object instance)) return (TSource) instance;
        throw new
            InstanceNotFoundException($"Instance of type {typeof(TSource).Name} not found, provide it with the {nameof(Provide)} method first.");
    }
    
    public static TSource Retrieve<TSource>([NotNull] this TSource _) where TSource : new()
    {
        return Retrieve<TSource>();
    }
}