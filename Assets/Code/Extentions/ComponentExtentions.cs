using UnityEngine;

public static class ComponentExtentions
{
    public static T GetOrAddComponent<T>(this GameObject gameobject) where T : Component
    {
        if (gameobject.TryGetComponent(out T t))
        {
            return t;
        }
        else
        {
            return gameobject.AddComponent<T>();
        }
    }
}


