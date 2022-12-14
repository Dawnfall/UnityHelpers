using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Dawnfall.Helper
{
    public static class HelperSaveLoad
    {

        public static bool SaveJsonToPrefs<T>(T o, string key)
        {
            try
            {
                PlayerPrefs.SetString(key, JsonUtility.ToJson(o));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T LoadFromJsonPrefs<T>(string key, T defaultValue)
        {
            try
            {
                return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static void SaveToFileWithBinFormater<T>(T obj, string path, string name, string extention)
        {
            string filePath = System.IO.Path.Combine(path, name + "." + extention);
            FileStream file = File.Open(filePath, FileMode.Create);

            BinaryFormatter binFormater = new BinaryFormatter();
            binFormater.Serialize(file, obj);
            file.Close();
        }

        public static T LoadFromFileWithBinaryFormater<T>(string path, string name, string extention) where T : class
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


        public static void SaveTextureToPng(string pathWithoutExtention, Texture2D texture)
        {
            byte[] texData = texture.EncodeToPNG();
            File.WriteAllBytes(Path.Combine(Application.dataPath, pathWithoutExtention) + ".png", texData);
        }
        //public static string[] GetAllFileNames(string extension)
        //{
        //    ExternalData externalData = ExternalData.Instance;

        //    CheckForSaveFolder();
        //    return Directory.GetFiles(externalData.SavePath, "*." + externalData.SaveExtention);
        //}
        //public static void CheckForSaveFolder()
        //{
        //    ExternalData externalMaster = ExternalData.Instance;
        //    if (externalMaster.SavePath == "")
        //    {
        //        externalMaster.SavePath = System.IO.Path.Combine(Application.persistentDataPath, "Levels");
        //    }

        //    if (!Directory.Exists(externalMaster.SavePath))
        //        Directory.CreateDirectory(externalMaster.SavePath);
        //}

        //public static void SaveToFile<T>(T obj, string path, string name, string extention)
        //{
        //    CheckForSaveFolder();

        //    string filePath = System.IO.Path.Combine(path, name + "." + extention);
        //    FileStream file = File.Open(filePath, FileMode.Create);

        //    BinaryFormatter binFormater = new BinaryFormatter();
        //    binFormater.Serialize(file, obj);
        //    file.Close();
        //}
        //public static T LoadFile<T>(string path, string name, string extention) where T : class
        //{
        //    CheckForSaveFolder();
        //    string[] allFiles = Directory.GetFiles(path, name + "." + extention, SearchOption.TopDirectoryOnly);
        //    if (allFiles.Length == 0)
        //        return null;

        //    FileStream file = File.Open(allFiles[0], FileMode.Open);

        //    BinaryFormatter binFormatter = new BinaryFormatter();
        //    T loadedData = (T)binFormatter.Deserialize(file);
        //    file.Close();

        //    return loadedData;
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

    }
}