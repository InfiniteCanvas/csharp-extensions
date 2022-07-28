using UnityEngine;

namespace Common.UnityExtensions;

public static class GameObjectExtensions
{
    public static TComponent?
        GetComponentOrDefault<TComponent>(this GameObject gameObject, TComponent? defaultComponent = default)
        where TComponent : Component =>
        gameObject.GetComponent<TComponent>() ?? defaultComponent;

    public static void DespawnToPool(this GameObject gameObject)
    {
        PoolObject<GameObject> poolObject = gameObject.MakePoolable();
        poolObject.Despawn();
    }
}