using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

namespace helper
{
    public static class HelperSaveLoad
    {
        public static void CreateFolderIfNeeded(string path)
        {
            //ExternalData externalMaster = ExternalData.Instance;
            //if (externalMaster.SavePath == "")
            //{
            //    externalMaster.SavePath = System.IO.Path.Combine(Application.persistentDataPath, "Levels");
            //}

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static bool DeleteFile(string fileName) //TODO: trigger event
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                return true;
            }
            return false;
        }
        //folder must exist!
        public static void SaveToFile<T>(T obj, string path, string name, string extention)
        {
            string filePath = System.IO.Path.Combine(path, name + "." + extention);
            FileStream file = File.Open(filePath, FileMode.Create);

            BinaryFormatter binFormater = new BinaryFormatter();
            binFormater.Serialize(file, obj);
            file.Close();
        }
        //folder must exist!
        public static T LoadFile<T>(string path, string name, string extention) where T : class
        {
            string[] allFiles = Directory.GetFiles(path, name + "." + extention, SearchOption.TopDirectoryOnly);
            if (allFiles.Length == 0)
                return null;

            FileStream file = File.Open(allFiles[0], FileMode.Open);

            BinaryFormatter binFormatter = new BinaryFormatter();
            T loadedData = (T)binFormatter.Deserialize(file);
            file.Close();

            return loadedData;
        }

        public static void SaveTexture(string folder, string name, Texture2D texture)
        {
            byte[] texData = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + folder + name + ".png", texData);
        }

        public static List<T> LoadAssetsFromFolder<T>(string folder) where T : Object
        {
            string[] fileInfos = AssetDatabase.FindAssets("");

            List<T> allObjects = new List<T>();
            foreach (var infoGUID in fileInfos)
            {
                string info = AssetDatabase.GUIDToAssetPath(infoGUID);

                if (HelperCommon.isInArray<string>(info.Split('/'), folder))
                {
                    T newLoadedObject = AssetDatabase.LoadAssetAtPath(info, typeof(T)) as T;
                    if (newLoadedObject == null)
                        continue;

                    newLoadedObject.name = info.Substring(info.LastIndexOf("/") + 1);
                    allObjects.Add(newLoadedObject);
                }
            }
            return allObjects;
        }


    }
}

//public static string[] GetAllFileNames(string extension)
//{
//    ExternalData externalData = ExternalData.Instance;

//    CheckForSaveFolder();
//    return Directory.GetFiles(externalData.SavePath, "*." + externalData.SaveExtention);
//}


//public static bool DeleteLevel(string levelName) //TODO: trigger event
//{
//    ExternalMaster externalMaster = ExternalMaster.Instance;

//    CheckForSaveFolder();
//    string filePath = System.IO.Path.Combine(externalMaster.SavePath, levelName + "." + externalMaster.SaveExtention);

//    if (File.Exists(filePath))
//    {
//        File.Delete(filePath);

//        EventManager.TriggerEvent(EventNames.External_Files_Changed);
//        return true;
//    }
//    return false;
//}

//public static void SaveAssetToFile<T>(T asset, string folder) where T : ScriptableObject
//{
//            else if (Path.GetExtension(path) != "")
//    {
//        path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
//    }

//    string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

//    AssetDatabase.CreateAsset(asset, assetPathAndName);

//    AssetDatabase.SaveAssets();
//    AssetDatabase.Refresh();
//    EditorUtility.FocusProjectWindow();
//    Selection.activeObject = asset;
//}