using System.Collections.Generic;
using UnityEngine;

namespace GreatDevResh.AppliedStuff
{
    public static class ObjectPool
    {
        private static Dictionary<string, List<Transform>> Store;

        public static Transform GetObj(Transform prefab)
        {
            if (Store is null)
                Store = new Dictionary<string, List<Transform>>();
            
            string name = prefab.name;
            if (!Store.ContainsKey(name))
                Store.Add(name, new List<Transform>());
            if (Store[name].Count == 0)
            {
                Transform obj = Object.Instantiate(prefab);
                obj.name = name;
                Store[name].Add(obj);
            }

            Transform res = Store[name][^1];
            Store[name].RemoveAt(Store[name].Count - 1);
            res.gameObject.SetActive(true);
            return res;
        }

        public static void ReturnObj(Transform obj)
        {
            if (Store is null)
                Store = new Dictionary<string, List<Transform>>();
            
            string name = obj.name;
            if (!Store.ContainsKey(name))
                Store.Add(name, new List<Transform>());
            obj.gameObject.SetActive(false);
            Store[name].Add(obj);
            // Debug.Log(Store.Count);
        }
    }
}