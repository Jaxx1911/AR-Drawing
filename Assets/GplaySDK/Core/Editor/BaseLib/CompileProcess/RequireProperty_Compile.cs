// Filename: RequireProperty_Compile.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:56 AM 05/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Reflection;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.LoadInfo;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.Core.Editor.BaseLib.Attribute;
using UnityEngine;

namespace GplaySDK.Core.Editor.BaseLib.CompileProcess
{
    internal static class RequireProperty_Compile
    {
        [EditorInitialize(10, EditorInitializeLoadType.OnCompileInitialize)]
        private static void Compile()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var buildList = new Dictionary<string, RequirePropertyInfo>();
            foreach (var assembly in assemblies)
            {
                var allTypes = assembly.GetTypes();
                foreach (var type in allTypes)
                {
                    var allProperties = type.GetProperties(BindingFlags.Static
                                                           | BindingFlags.Public
                                                           | BindingFlags.NonPublic);
                    foreach (var property in allProperties)
                    {
                        var requirePropertyAttribute = property.GetCustomAttribute<RequirePropertyAttribute>();
                        if (requirePropertyAttribute == null)
                        {
                            continue;
                        }

                        var requirePropertyInfo = new RequirePropertyInfo(assembly.GetName().Name,
                            type.FullName, property.Name);
                        buildList.Add(requirePropertyAttribute.Name, requirePropertyInfo);
                    }
                }
            }
#if G_LOG_LOW_LEVEL
            Debug.Log("Require Property Build List: " + buildList.ToJson(true));
#endif
            StreamingAssetUtils.SaveToStreamingAsset(RequirePropertyInfo.FileName,
                buildList.ToJson());
        }
    }
}