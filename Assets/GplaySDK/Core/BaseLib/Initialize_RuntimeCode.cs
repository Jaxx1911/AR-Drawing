// Filename: Initialize_RuntimeCode.cs
// Purpose: To start initialize all runtime code which have Attribute [RuntimeInitializeAttribute]
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:49 PM 21/09/2023
// 
// Notes: Support RuntimeInitializeLoadType:
//        - AfterAssembliesLoaded
//        - BeforeSceneLoad
//        - AfterSceneLoad
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.LoadInfo;
using GplaySDK.Core.BaseLib.Utils;
using UnityEngine;

namespace GplaySDK.Core.BaseLib
{
    public static class Initialize_RuntimeCode
    {
        private static RuntimeInitializeInfo.RuntimeInitializeInfoPack _allInitializer;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void AfterAssembliesLoaded()
        {
            _allInitializer = StreamingAssetUtils.ReadAllText(RuntimeInitializeInfo.FileName)
                .FromJson<RuntimeInitializeInfo.RuntimeInitializeInfoPack>();

#if G_LOG_LOW_LEVEL
            Debug.Log("All Initializer: "
                      + (_allInitializer.afterAssembliesLoaded.Length
                         + _allInitializer.beforeSceneLoad.Length
                         + _allInitializer.afterSceneLoad.Length));
            Debug.Log("After Asm Initialize\nCount: " + _allInitializer.afterAssembliesLoaded.Length);
#endif
            foreach (var initializer in _allInitializer.afterAssembliesLoaded)
            {
                initializer.Initialize();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoad()
        {
#if G_LOG_LOW_LEVEL
            Debug.Log("Before Scene Initialize\nCount: " + _allInitializer.beforeSceneLoad.Length);
#endif
            foreach (var initializer in _allInitializer.beforeSceneLoad)
            {
                initializer.Initialize();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
#if G_LOG_LOW_LEVEL
            Debug.Log("After Scene Initialize\nCount: " + _allInitializer.afterSceneLoad.Length);
#endif
            foreach (var initializer in _allInitializer.afterSceneLoad)
            {
                initializer.Initialize();
            }
        }
    }
}