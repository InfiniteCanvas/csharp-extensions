using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.UnityExtensions;

public class PoolObject<TSource> where TSource : new()
{
    public PoolObject(TSource value) => Value = value;

    public TSource Value { get; }

    public static implicit operator TSource(PoolObject<TSource> poolObject) => poolObject.Value;
}

public class Pool<TSource> where TSource : new()
{
    private readonly Stack<PoolObject<TSource>>   _availablePoolObjects;
    private readonly Func<TSource>                _factory;
    private readonly HashSet<PoolObject<TSource>> _totalPoolObjects;

    internal Pool(Func<TSource> factory)
    {
        _factory = factory;
        _availablePoolObjects = new Stack<PoolObject<TSource>>();
        _totalPoolObjects = new HashSet<PoolObject<TSource>>();
        ObjectAddedEvent += o => _totalPoolObjects.Add(o);
    }

    public int Count => _availablePoolObjects.Count;

    public event Action<PoolObject<TSource>>? ObjectAddedEvent;
    public event Action<PoolObject<TSource>>? SpawnEvent;
    public event Action<PoolObject<TSource>>? DespawnEvent;

    private void OnObjectAdded(PoolObject<TSource> obj) => ObjectAddedEvent?.Invoke(obj);

    private void OnSpawn(PoolObject<TSource> obj) => SpawnEvent?.Invoke(obj);

    private void OnDespawn(PoolObject<TSource> obj) => DespawnEvent?.Invoke(obj);

    private PoolObject<TSource> SpawnFromPool()
    {
        PoolObject<TSource>? obj = _availablePoolObjects.Pop();
        OnSpawn(obj);
        return obj;
    }

    private PoolObject<TSource> SpawnNewObject()
    {
        var obj = new PoolObject<TSource>(_factory());
        OnObjectAdded(obj);
        OnSpawn(obj);
        return obj;
    }

    public PoolObject<TSource> Spawn() =>
        _availablePoolObjects.Count switch
        {
            0 => SpawnNewObject(),
            _ => SpawnFromPool(),
        };

    public IEnumerable<PoolObject<TSource>> Spawn(int count)
    {
        for (var i = 0; i < count; i++) yield return Spawn();
    }

    public bool Despawn(PoolObject<TSource> obj)
    {
        if (IsAvailable(obj))
        {
            OnDespawn(obj);
            _availablePoolObjects.Push(obj);
            return true;
        }

        return false;
    }

    public bool Despawn(IEnumerable<PoolObject<TSource>> objs) => objs.Select(Despawn).All(despawned => despawned);

    public void DespawnAll()
    {
        foreach (PoolObject<TSource> obj in _totalPoolObjects) Despawn(obj);
    }

    public bool Add(TSource source)
    {
        PoolObject<TSource> obj = source.MakePoolable();

        if (InPool(obj)) return false;

        OnObjectAdded(obj);
        _availablePoolObjects.Push(obj);
        return true;
    }

    public bool InPool(PoolObject<TSource> obj) => _totalPoolObjects.Contains(obj);

    public bool IsAvailable(PoolObject<TSource> obj) => _availablePoolObjects.Contains(obj);

    public bool Add(IEnumerable<TSource> sources) => sources.Select(Add).All(added => added);

    public void Clear() => _availablePoolObjects.Clear();

    public void Create(int count)
    {
        var i = 0;
        while (i < count)
        {
            Add(new PoolObject<TSource>(_factory()));
            i++;
        }
    }
}

public static class Pools<TSource> where TSource : new()
{
    public static Dictionary<string, Pool<TSource>> PoolsDict { get; } = new();

    public static IEnumerable<string> PoolNames => PoolsDict.Keys;

    public static Pool<TSource> GetOrCreatePool(string name = "", Func<TSource>? factory = null) =>
        PoolsDict.TryGetValue(name, out Pool<TSource> pool) switch
        {
            true  => pool!,
            false => CreateNewPool(name, factory ?? (() => new TSource())),
        };

    public static Pool<TSource> CreateNewPool(string name = "", Func<TSource>? factory = null)
    {
        var pool = new Pool<TSource>(factory ?? (() => new TSource()));
        PoolsDict.Add(name, pool);
        return pool;
    }

    public static Pool<TSource> CreateNewPool(string name, Func<TSource> factory, int initialSize)
    {
        Pool<TSource> pool = CreateNewPool(name, factory);
        pool.Create(initialSize);
        return pool;
    }
}