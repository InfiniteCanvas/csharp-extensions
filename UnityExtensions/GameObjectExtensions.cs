using UnityEngine;

namespace Common.UnityExtensions
{
    public static class GameObjectExtensions
    {
        public static TComponent?
            GetComponentOrDefault<TComponent>(this GameObject gameObject, TComponent? defaultComponent = default)
            where TComponent : Component
        {
            return gameObject.GetComponent<TComponent>() ?? defaultComponent;
        }
    }
}