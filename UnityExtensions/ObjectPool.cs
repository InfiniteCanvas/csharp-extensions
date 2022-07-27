using System;
using System.Collections.Generic;

namespace Common.UnityExtensions;

public class PoolObject<TSource> where TSource : new()
{
    private readonly TSource _value;

    public PoolObject(TSource value) => _value = value;

    public static implicit operator TSource(PoolObject<TSource> poolObject) => poolObject._value;
}

public static class Pool<TSource> where TSource : new()
{
    private static readonly Stack<PoolObject<TSource>> ObjectStack;

    static Pool() => ObjectStack = new Stack<PoolObject<TSource>>();

    public static event Action<PoolObject<TSource>>? ObjectAddedEvent;
    public static event Action<PoolObject<TSource>>? SpawnEvent;
    public static event Action<PoolObject<TSource>>? DespawnEvent;

    private static void OnObjectAdded(PoolObject<TSource> obj) => ObjectAddedEvent?.Invoke(obj);

    private static void OnSpawn(PoolObject<TSource> obj) => SpawnEvent?.Invoke(obj);

    private static void OnDespawn(PoolObject<TSource> obj) => DespawnEvent?.Invoke(obj);

    private static PoolObject<TSource> SpawnFromPool()
    {
        PoolObject<TSource>? obj = ObjectStack.Pop();
        OnSpawn(obj);
        return obj;
    }

    private static PoolObject<TSource> SpawnNewObject(TSource? source = default)
    {
        var obj = new PoolObject<TSource>(source ?? new TSource());
        OnObjectAdded(obj);
        OnSpawn(obj);
        return obj;
    }

    public static PoolObject<TSource> Spawn(TSource? source = default) =>
        (source, ObjectStack.Count) switch
        {
            (not null, _) => SpawnNewObject(source),
            (_, 0)        => SpawnNewObject(),
            (_, _)        => SpawnFromPool(),
        };

    public static void Despawn(PoolObject<TSource> obj)
    {
        OnDespawn(obj);
        ObjectStack.Push(obj);
    }

    public static void Add(TSource source)
    {
        PoolObject<TSource> obj = source.Pool();
        OnObjectAdded(obj);
        ObjectStack.Push(obj);
    }
}