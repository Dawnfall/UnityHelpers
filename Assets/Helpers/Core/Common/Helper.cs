using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Dawnfall.Helper
{
    public static class Helper
    {
        public static void Swap<T>(IList<T> array, int i1, int i2)
        {
            T temp1 = array[i1];
            array[i1] = array[i2];
            array[i2] = temp1;
        }

        public static T[] ReverseArray<T>(T[] arr)
        {
            T[] reversedArr = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                reversedArr[i] = arr[arr.Length - i - 1];
            return reversedArr;
        }

        public static T[] SubArray<T>(T[] arr, int startIndex, int length)
        {
            T[] subArr = new T[length];
            for (int i = 0; i < length; i++)
                subArr[i] = arr[i + startIndex];
            return subArr;
        }

        public static bool IsInArray<T>(IEnumerable<T> arrStr, T item)
        {
            if (arrStr == null)
                return false;
            foreach (T str in arrStr)
                if (str.Equals(item))
                    return true;
            return false;
        }

        public static bool AddIfType<T>(object obj, List<T> list) where T : class
        {
            T castObj = obj as T;
            if (castObj != null)
            {
                list.Add(castObj);
                return true;
            }
            return false;
        }

        //not necceserily correct
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return binForm.Deserialize(memStream);
        }
    }
}

//public static bool AddIfType<T>(AActorData actorData, List<T> list) where T : AActorData
//{
//    T castActor = actorData as T;
//    if (castActor != null)
//    {
//        list.Add(castActor);
//        return true;
//    }
//    return false;
//}