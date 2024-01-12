// Filename: RequirePropertyAttribute.cs
// Purpose: RequirePropertyAttribute is a base class for all attribute that define a property need to implement in the project.
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 2:28 PM 22/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Reflection;
using GplaySDK.Core.BaseLib.LoadInfo;
using GplaySDK.Core.BaseLib.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    [MeansImplicitUse]
    public abstract class RequirePropertyAttribute : System.Attribute
    {
        private static readonly Dictionary<string, PropertyInfo> PropertyInfos = new Dictionary<string, PropertyInfo>();
        public string Name { get; }

        private readonly ValueType _valueType;

        protected RequirePropertyAttribute(string name, ValueType valueType)
        {
            Name = name;
            _valueType = valueType;
        }

        internal static void Initialize(Dictionary<string, RequirePropertyInfo> dataMap)
        {
#if G_SKIP_REQUIRE_PROPERTY
#if LOG_VERBOSE
            "Skip Init Require Property".Log();
#endif
            return;
#endif
            PropertyInfos.Clear();
            foreach (var initInfo in dataMap)
            {
                PropertyInfos.Add(initInfo.Key, initInfo.Value.Initialize());
            }
#if G_LOG_LOW_LEVEL
            Debug.Log($"Init Require Property\n{dataMap.ToJson(true)}");
#endif
        }


        protected static bool GetBoolValue(string name)
        {
#if G_SKIP_REQUIRE_PROPERTY
#if LOG_VERBOSE
            $"Skip Get Require Property| {name}: {default}".Log();
#endif
            return default;
#endif
            return (bool) PropertyInfos[name].GetValue(null);
        }

        protected static void SetBoolValue(string name, bool value)
        {
#if G_SKIP_REQUIRE_PROPERTY
#if LOG_VERBOSE
            $"Skip Set Require Property| {name}".Log();
#endif
            return;
#endif
            PropertyInfos[name].SetValue(null, value);
        }

        protected static string GetStringValue(string name)
        {
#if G_SKIP_REQUIRE_PROPERTY
#if LOG_VERBOSE
            $"Skip Get Require Property| {name}: {default}".Log();
#endif
            return default;
#endif
            return (string) PropertyInfos[name].GetValue(null);
        }

        protected static void SetStringValue(string name, string value)
        {
#if G_SKIP_REQUIRE_PROPERTY
#if LOG_VERBOSE
            $"Skip Set Require Property| {name}".Log();
#endif
            return;
#endif
            PropertyInfos[name].SetValue(null, value);
        }

        protected static int GetIntValue(string name)
        {
#if G_SKIP_REQUIRE_PROPERTY
#if LOG_VERBOSE
            $"Skip Get Require Property| {name}: {default}".Log();
#endif
            return default;
#endif
            return (int) PropertyInfos[name].GetValue(null);
        }

        protected static void SetIntValue(string name, int value)
        {
#if G_SKIP_REQUIRE_PROPERTY
#if LOG_VERBOSE
            $"Skip Set Require Property| {name}".Log();
#endif
            return;
#endif
            PropertyInfos[name].SetValue(null, value);
        }

        protected static float GetFloatValue(string name)
        {
#if G_SKIP_REQUIRE_PROPERTY
#if LOG_VERBOSE
            $"Skip Get Require Property| {name}: {default}".Log();
#endif
            return default;
#endif
            return (float) PropertyInfos[name].GetValue(null);
        }

        protected static void SetFloatValue(string name, float value)
        {
#if G_SKIP_REQUIRE_PROPERTY
#if LOG_VERBOSE
            $"Skip Set Require Property| {name}".Log();
#endif
            return;
#endif
            PropertyInfos[name].SetValue(null, value);
        }


        public bool ValidatorType(Type declaringType)
        {
            switch (_valueType)
            {
                case ValueType.Int:
                    if (declaringType != typeof(int))
                        return false;
                    break;
                case ValueType.String:
                    if (declaringType != typeof(string))
                        return false;
                    break;
                case ValueType.Bool:
                    if (declaringType != typeof(bool))
                        return false;
                    break;
                case ValueType.Float:
                    if (declaringType != typeof(float))
                        return false;
                    break;
                case ValueType.None:
                default:
                    return false;
            }

            return true;
        }

        protected enum ValueType
        {
            None = 0,
            Int = 1,
            String = 2,
            Bool = 3,
            Float = 4
        }
    }
}