using System;

namespace Common.UnityExtensions;

public static class ObjectPoolExtensions
{
    public static PoolObject<TSource> Pool<TSource>(this TSource source) where TSource : new() => new(source);

    public static PoolObject<TSource> Pool<TSource>(this TSource source, Action<TSource> initialize)
        where TSource : new()
    {
        initialize(source);
        return new PoolObject<TSource>(source);
    }

    public static void Despawn<TSource>(this PoolObject<TSource> poolObject) where TSource : new() =>
        UnityExtensions.Pool<TSource>.Despawn(poolObject);

    public static void AddObjectCreationEvent<TSource>(this PoolObject<TSource>    poolObject,
                                                       Action<PoolObject<TSource>> action) where TSource : new() =>
        UnityExtensions.Pool<TSource>.ObjectAddedEvent += action;

    public static void AddSpawnEvent<TSource>(this PoolObject<TSource>    poolObject,
                                              Action<PoolObject<TSource>> action) where TSource : new() =>
        UnityExtensions.Pool<TSource>.SpawnEvent += action;

    public static void AddDespawnEvent<TSource>(this PoolObject<TSource>    poolObject,
                                                Action<PoolObject<TSource>> action) where TSource : new() =>
        UnityExtensions.Pool<TSource>.DespawnEvent += action;
}