using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dawnfall.Helper.Editor
{
    public static class HelperEditor
    {
        public static List<T> LoadObjectsFromFolder<T>(string[] folders) where T : Object
        {
            string searchFilter = "t:" + typeof(T).ToString();
            Debug.Log(searchFilter);

            string[] guids = (folders == null) ? AssetDatabase.FindAssets(searchFilter) : AssetDatabase.FindAssets(searchFilter, folders);
            Debug.Log("paths count: " + guids.Length);
            if (guids.Length > 0)
                Debug.Log(guids[0]);
            List<T> result = new List<T>();
            foreach (string id in guids)
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(id));
                if (asset != null)
                    result.Add(asset);
            }

            Debug.Log(result.Count);
            return result;
        }

    }
}