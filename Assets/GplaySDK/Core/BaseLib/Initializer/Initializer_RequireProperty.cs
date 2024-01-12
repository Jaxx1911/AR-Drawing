// Filename: Initializer_RequireProperty.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 10:11 AM 29/09/2023
// 
// Notes:
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

namespace GplaySDK.Core.BaseLib.Initializer
{
    internal static class Initializer_RequireProperty
    {
        [RuntimeInitialize(-100, RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initializer()
        {
#if G_SKIP_REQUIRE_PROPERTY
            return;
#endif
            var requirePropertyInitializeInfo = StreamingAssetUtils
                .ReadAllText(RequirePropertyInfo.FileName)
                .FromJson<Dictionary<string, RequirePropertyInfo>>();
            RequirePropertyAttribute.Initialize(requirePropertyInitializeInfo);
        }
    }
}