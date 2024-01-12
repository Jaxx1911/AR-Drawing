// Filename: RequirePropertyInfo.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:45 PM 05/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Reflection;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Utils;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.LoadInfo
{
    [Serializable]
    public class RequirePropertyInfo
    {
        public const string FileName = "RequirePropertyInitiliazer.inz";
        public string AssemblyName { get; private set; }
        public string TypeName { get; private set; }
        public string PropertyName { get; private set; }


        private RequirePropertyInfo()
        {
        }

        public RequirePropertyInfo(string assemblyName, string typeName, string propertyName)
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
            PropertyName = propertyName;
        }

        public PropertyInfo Initialize()
        {
            var property = AppDomain.CurrentDomain.Load(AssemblyName)
                .GetType(TypeName)
                .GetProperty(PropertyName);
            if (property == null)
            {
                throw new System.Exception($"Property {PropertyName} not found in type {TypeName}");
            }

#if G_LOG_LOW_LEVEL
            Debug.Log("Require Property Initialized: " + TypeName + "." + PropertyName + "| " +
                      property.GetValue(null));
#endif
            return property;
        }
    }
}