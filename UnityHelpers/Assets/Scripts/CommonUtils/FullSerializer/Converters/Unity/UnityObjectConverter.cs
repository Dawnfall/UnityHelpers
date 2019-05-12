using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace helper.FullSerializer
{
    public class UnityObjectConverter : fsConverter
    {
        public override bool CanProcess(Type type)
        {
            return typeof(UnityEngine.Object).IsAssignableFrom(type);
        }
        public override object CreateInstance(fsData data, Type storageType)
        {
            return null;
        }

        public override bool RequestCycleSupport(Type storageType)
        {
            return false;
        }
        public override bool RequestInheritanceSupport(Type storageType)
        {
            return base.RequestInheritanceSupport(storageType);
        }

        public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType, object other)
        {
            IUnityObjectRegister unityObjectRegister = other as IUnityObjectRegister; //TODO: maybe? do interface for this
            if (unityObjectRegister != null) 
                instance = unityObjectRegister.GetRegisteredUO((int)data.AsInt64);
            else
                instance = null;      
            
            return fsResult.Success;
        }
        public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType, object other)
        {
            IUnityObjectRegister unityObjectRegister = other as IUnityObjectRegister; //TODO: maybe? do interface for this
            if (unityObjectRegister != null)
                serialized = new fsData(unityObjectRegister.RegisterUnityObject(instance as UnityEngine.Object));
            else
                serialized = new fsData(-1);
            return fsResult.Success;
        }
    }

    public interface IUnityObjectRegister
    {
        int RegisterUnityObject(UnityEngine.Object uo);
        UnityEngine.Object GetRegisteredUO(int index);
    }
}

