using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper
{
    public static class HelperUnity
    {
        public static GameObject FindGameObject(string name)
        {
            GameObject go = GameObject.Find(name);

            if (go == null)
                Debug.Log("GameObject by the name: " + name + " cannot be found!");

            return GameObject.Find("name");
        }

        public static GameObject FindChildGO(string childName, GameObject parent)
        {
            if (parent == null)
            {
                Debug.Log("Parent is null!Cannot look for children");
                return null;
            }

            Transform child = parent.transform.Find(childName);
            if (child == null)
            {
                Debug.Log("No child of name: " + childName + " exists on gameObject: " + parent);
                return null;
            }
            return child.gameObject;
        }

        //also includes inactive
        public static T FindComponentOnChildren<T>(Transform parent) where T : Component
        {
            T comp = parent.GetComponent<T>();
            if (comp != null)
                return comp;

            foreach (Transform child in parent)
                return FindComponentOnChildren<T>(child);

            return null;
        }
        public static T FindComponentOnChild<T>(string childName, GameObject parent) where T : Component
        {
            GameObject childGO = FindChildGO(childName, parent);

            if (childGO != null)
                return childGO.GetComponent<T>();
            return null;
        }

        public static T FindComponentOnGameObject<T>(string goName) where T : Component
        {
            GameObject go = GameObject.Find(goName);
            if (go != null)
            {
                T comp = go.GetComponent<T>();

                if (comp == null)
                    Debug.Log("Component of type" + typeof(T).ToString() + " doesnt exist on Gameobject: " + goName);
                return comp;
            }

            return null;
        }
        public static List<T> GetInterfaces<T>(GameObject objectToSearch) where T : class
        {
            MonoBehaviour[] list = objectToSearch.GetComponents<MonoBehaviour>();
            List<T> resultList = new List<T>();
            foreach (MonoBehaviour mb in list)
                if (mb is T)
                    resultList.Add((T)((System.Object)mb));
            return resultList;
        }

        public static List<T> Spawn<T>(T prefab, Vector3 pos, Transform parent, int count) where T : MonoBehaviour
        {
            List<T> spawns = new List<T>();
            for (int i = 0; i < count; i++)
                spawns.Add(GameObject.Instantiate<T>(prefab, pos, Quaternion.identity, parent));

            return spawns;
        }

        public static T Spawn<T>(T prefab, Vector3 pos, Transform parent) where T : MonoBehaviour
        {
            T newGO = GameObject.Instantiate(prefab, pos, Quaternion.identity).GetComponent<T>();
            newGO.transform.SetParent(parent);

            return newGO;
        }

        public static void GetChildrenTransforms(Transform t, List<Transform> childComponents, bool doRecursive = false)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);
                childComponents.Add(child);

                if (doRecursive)
                    GetChildrenTransforms(child, childComponents, doRecursive);
            }
        }
        public static List<Transform> GetChildrenTransforms(Transform t, bool doRecursive = false)
        {
            List<Transform> children = new List<Transform>();
            GetChildrenTransforms(t, children);
            return children;
        }

        public static void GetAllChildrenComponents<T>(Transform t, List<T> childComponents, bool doRecursive = true) where T : Component
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);

                T c = child.GetComponent<T>();
                if (c != null)
                    childComponents.Add(c);

                if (doRecursive)
                    GetAllChildrenComponents<T>(child, childComponents, doRecursive);
            }
        }

        public static void SetLayerToAllChildren(GameObject parent, int layer)
        {
            parent.layer = layer;
            for (int i = 0; i < parent.transform.childCount; i++)
                HelperUnity.SetLayerToAllChildren(parent.transform.GetChild(i).gameObject, layer);
        }



        /// <summary>
        /// Exits the application in approptiate way
        /// </summary>
        public static void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }

        public static void DestroyAllChildren(Transform t)
        {
            while (t.childCount > 0)
            {
                Transform childT = t.GetChild(t.childCount - 1);

                childT.SetParent(null);

#if UNITY_EDITOR
                if (Application.isPlaying)
                    GameObject.Destroy(childT.gameObject);
                else
                    GameObject.DestroyImmediate(childT.gameObject);
#else
            GameObject.Destroy(childT.gameObject);
#endif
            }
        }

        public static void DestroyAllChildrenOfType<T>(Transform t, bool isRecursive) where T : Component
        {
            T[] allComponents = (isRecursive) ? t.GetComponentsInChildren<T>() : t.GetComponents<T>();

            foreach (T component in allComponents)
            {
                if (component != null && component.transform.parent != null)
                {
                    component.transform.SetParent(null);

#if UNITY_EDITOR
                    if (Application.isPlaying)
                        GameObject.Destroy(component.gameObject);
                    else
                        GameObject.DestroyImmediate(component.gameObject);
#else
                    GameObject.Destroy(component.gameObject);
#endif
                }
            }
        }

        //TODO: only returns one if multi touched
        public static List<Vector2> GetClickedOrTouchedPositions()
        {
            List<Vector2> positions = new List<Vector2>();
            if (Input.GetMouseButtonDown(0))
            {
                positions.Add(Input.mousePosition);
            }
            else if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                    positions.Add(Input.GetTouch(0).position);
            }
            return positions;
        }

        public static RaycastHit2D RaycastMouse2D(Camera camera)
        {
            Vector3 camWorld = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 position = new Vector2(camWorld.x, camWorld.y);

            return Physics2D.Raycast(position, Vector2.zero, 0f);
        }
        public static List<RaycastHit2D> Racast2DArk(Vector3 position, Vector3 forward, float angle, int rayCount, float distance, bool doAllPerRay)//TODO layermask
        {
            List<RaycastHit2D> hitList = new List<RaycastHit2D>();

            foreach (Vector2 dir in HelperMath.GetArkVectors(forward,Vector3.forward, angle, rayCount))
            {
                if (doAllPerRay)
                    hitList.AddRange(Physics2D.RaycastAll(position, dir, distance));
                else
                    hitList.Add(Physics2D.Raycast(position, dir, distance));
            }
            return hitList;
        }
        public static void ScaleAroundPoint(Transform target, Vector3 localPivot, Vector3 newScale)
        {
            float dx = (target.localPosition.x - localPivot.x) * (newScale.x / target.localScale.x);
            float dy = (target.localPosition.y - localPivot.y) * (newScale.y / target.localScale.y);
            float dz = (target.localPosition.z - localPivot.z) * (newScale.z / target.localScale.z);

            target.localPosition = new Vector3(localPivot.x + dx, localPivot.y + dy, localPivot.z + dz);
            target.transform.localScale = newScale;
        }

        public static void PrintCollection<T>(IEnumerable<T> enumerable)
        {
            foreach (var a in enumerable)
            {
                Debug.Log(a.ToString());
            }
        }

        public static Texture2D GenerateGrayScaleTexture(Map2D noiseMap, FilterMode filterMode)
        {
            Texture2D newTexture = new Texture2D(noiseMap.m_width, noiseMap.m_height);

            Color[] pixels = new Color[noiseMap.m_width * noiseMap.m_height];
            for (int i = 0; i < noiseMap.m_width * noiseMap.m_height; i++)
                pixels[i] = new Color(noiseMap.m_noiseMap[i], noiseMap.m_noiseMap[i], noiseMap.m_noiseMap[i]);

            newTexture.SetPixels(pixels);
            newTexture.filterMode = filterMode;
            newTexture.wrapMode = TextureWrapMode.Clamp;
            newTexture.Apply();

            return newTexture;
        }
        public static Texture2D GenerateRgbTexture(Map2D textureA, Map2D textureB, Map2D textureC, FilterMode filterMode)
        {
            Texture2D newTexture = new Texture2D(textureA.m_width, textureA.m_height);

            Color[] pixels = new Color[textureA.m_width * textureA.m_height];
            for (int i = 0; i < textureA.m_width * textureA.m_height; i++)
                pixels[i] = new Color(textureA.m_noiseMap[i], textureB.m_noiseMap[i], textureB.m_noiseMap[i]);

            newTexture.SetPixels(pixels);
            newTexture.filterMode = filterMode;
            newTexture.wrapMode = TextureWrapMode.Clamp;
            newTexture.Apply();

            return newTexture;
        }


    }
}