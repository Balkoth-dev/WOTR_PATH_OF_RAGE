using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using System;
using System.Linq;

namespace WOTR_PATH_OF_RAGE
{
    static class Extensions
    {
        public static void RemoveComponents<T>(this BlueprintScriptableObject obj) where T : BlueprintComponent
        {
            var components_to_remove = obj.GetComponents<T>().ToArray();
            foreach (var c in components_to_remove)
            {
                obj.SetComponents(obj.ComponentsArray.RemoveFromArray(c));
            }
        }
        public static T[] RemoveFromArray<T>(this T[] array, T value)
        {
            var list = array.ToList();
            return list.Remove(value) ? list.ToArray() : array;
        }
        public static void AddComponent<T>(this BlueprintScriptableObject obj, Action<T> init = null) where T : BlueprintComponent, new()
        {
            obj.SetComponents(obj.ComponentsArray.AppendToArray(Create(init)));
        }
        public static T[] AppendToArray<T>(this T[] array, T value)
        {
            var len = array.Length;
            var result = new T[len + 1];
            Array.Copy(array, result, len);
            result[len] = value;
            return result;
        }
        public static T Create<T>(Action<T> init = null) where T : new()
        {
            var result = new T();
            init?.Invoke(result);
            return result;
        }
        public static T EditComponent<T>(this BlueprintScriptableObject obj, Action<T> build) where T : BlueprintComponent
        {
            var component = obj.GetComponent<T>();
            build(component);
            return component;
        }
        public static T GetComponent<T>(this T obj, Action<T> init)
        {
            init?.Invoke(obj);
            return obj;
        }

    }
}
