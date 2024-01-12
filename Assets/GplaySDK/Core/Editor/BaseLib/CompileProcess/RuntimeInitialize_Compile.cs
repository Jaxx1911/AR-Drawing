// Filename: RuntimeInitialize_Compile.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:13 AM 06/10/2023
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
    internal static class RuntimeInitialize_Compile
    {
        [EditorInitialize(5, EditorInitializeLoadType.OnCompileInitialize)]
        private static void Compile()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var buildListAfterAssembliesLoaded = new List<RuntimeInitializeInfo>();
            var buildListBeforeSceneLoad = new List<RuntimeInitializeInfo>();
            var buildListAfterSceneLoad = new List<RuntimeInitializeInfo>();
            foreach (var assembly in assemblies)
            {
                var allTypes = assembly.GetTypes();
                foreach (var type in allTypes)
                {
                    var allMethods = type.GetMethods(
                        BindingFlags.Static
                        | BindingFlags.Public
                        | BindingFlags.NonPublic);
                    foreach (var method in allMethods)
                    {
                        var runtimeInitializeAttribute = method.GetCustomAttribute<RuntimeInitializeAttribute>();
                        if (runtimeInitializeAttribute == null)
                        {
                            continue;
                        }

                        var runtimeInitializeInfo = new RuntimeInitializeInfo(
                            assembly.GetName().Name,
                            type.FullName,
                            method.Name,
                            runtimeInitializeAttribute.Order);
                        switch (runtimeInitializeAttribute.LoadType)
                        {
                            case RuntimeInitializeLoadType.AfterAssembliesLoaded:
                                buildListAfterAssembliesLoaded.Add(runtimeInitializeInfo);
                                break;
                            case RuntimeInitializeLoadType.BeforeSceneLoad:
                                buildListBeforeSceneLoad.Add(runtimeInitializeInfo);
                                break;
                            case RuntimeInitializeLoadType.AfterSceneLoad:
                                buildListAfterSceneLoad.Add(runtimeInitializeInfo);
                                break;
                        }
                    }
                }
            }
            //Sort all
            buildListAfterAssembliesLoaded.Sort((a, b) => a.Order.CompareTo(b.Order));
            buildListBeforeSceneLoad.Sort((a, b) => a.Order.CompareTo(b.Order));
            buildListAfterSceneLoad.Sort((a, b) => a.Order.CompareTo(b.Order));
#if G_LOG_LOW_LEVEL
            Debug.Log(
                $@"Runtime Initialize Build List:
After Asm
{buildListAfterAssembliesLoaded.ToJson(true)}
------
Before Scene
{buildListBeforeSceneLoad.ToJson(true)}
------
After Scene
{buildListAfterSceneLoad.ToJson(true)}");
#endif
            var runtimeInitializeInfoPack = new RuntimeInitializeInfo.RuntimeInitializeInfoPack(
                buildListAfterAssembliesLoaded.ToArray(),
                buildListBeforeSceneLoad.ToArray(),
                buildListAfterSceneLoad.ToArray());
            StreamingAssetUtils.SaveToStreamingAsset(RuntimeInitializeInfo.FileName,
                runtimeInitializeInfoPack.ToJson());
        }
    }
}