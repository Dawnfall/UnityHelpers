using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Dawnfall.Helper
{
    public static class HelperReflection
    {
        public static Assembly[] getAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
        public static List<Type> getAllTypesInAssemblies(Assembly[] assemblies)
        {
            List<Type> types = new List<Type>();
            foreach (var assemblie in assemblies)
            {
                foreach (var type in assemblie.GetTypes())
                    types.Add(type);
            }

            return types;
        }
        public static List<Type> getAllInheritedTypesInAssemblies(Assembly[] assemblies,Type[] subTypes, bool doIncludeAbstract) 
        {
            List<Type> allSubTypes = new List<Type>();
            foreach (Assembly a in assemblies)
            {
                foreach (Type t in a.GetTypes())
                {
                    if ((doIncludeAbstract || (!doIncludeAbstract && !t.IsAbstract)) && IsTypeInheritingFromSubTypes(subTypes, t))
                    {
                        allSubTypes.Add(t);
                    }
                }
            }
            return allSubTypes;

        }

        public static bool IsTypeInheritingFromSubTypes(Type[] subTypes, Type inheritedType)
        {
            foreach (Type subType in subTypes)
            {
                if (!subType.IsAssignableFrom(inheritedType))
                    return false;
            }
            return true;
        }
        public static bool FindResultsForFirstIncludedSubtype<T>(Dictionary<Type, T> dict, Type searchType, out T result)
        {
            result = default(T);

            while (searchType != null)
            {
                if (dict.TryGetValue(searchType, out result))
                    return true;

                searchType = searchType.BaseType;
            }
            return false;
        }
        public static bool IsAssignableToGenericType(Type fromType, Type toType)
        {
            if (fromType == null)
                return false;
            if (fromType.IsGenericType && fromType.GetGenericTypeDefinition() == toType)
                return true;
            fromType = fromType.BaseType;
            return IsAssignableToGenericType(fromType, toType);
        }
        public static Type[] GenericSubclassArgumentTypes(Type genericSubclassDefinition, Type toCheck)
        {
            while (toCheck != null)
            {
                if (toCheck.IsGenericType && toCheck.GetGenericTypeDefinition() == genericSubclassDefinition)
                {
                    return toCheck.GetGenericArguments();
                }
                toCheck = toCheck.BaseType;
            }

            return null;
        }

        public static T GetAttributeOnType<T>(System.Type type, bool doInherit) where T : System.Attribute
        {
            object[] attributes = type.GetCustomAttributes(typeof(T), doInherit);
            if (attributes.Length == 0)
                return null;
            return attributes[0] as T;
        }

        public static List<System.Type> SelectAllOfGivenType(List<System.Type> allTypes, System.Type classType)
        {
            List<System.Type> selectedTypes = new List<System.Type>();
            foreach (var t in allTypes)
            {
                if (classType.IsAssignableFrom(t))
                    selectedTypes.Add(t);
            }
            return selectedTypes;
        }
    }
}