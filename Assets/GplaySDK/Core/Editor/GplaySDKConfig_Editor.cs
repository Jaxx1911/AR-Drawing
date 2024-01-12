// Filename: GplaySDKConfig_Editor.cs
// Purpose: 
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 2:32 PM 23/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Reflection;
using GplaySDK.Core.Editor.BaseLib.Attribute;
using UnityEditor;
using UnityEngine;

namespace GplaySDK.Core.Editor
{
    internal static class GplaySDKConfig_Editor
    {
        [EditorInitialize(-1000, EditorInitializeLoadType.OnSymbolInitialize)]
        private static void SymbolInitializer()
        {
            var config = GplaySDKConfig.GetConfig();
            if (config == null)
            {
                return;
            }

            config.InitializeSymbol();
        }

        [EditorInitialize(-10, EditorInitializeLoadType.OnCompileInitialize)]
        private static void CreateAsset()
        {
            var resourceGet = Resources.Load<GplaySDKConfig>("GplaySDKConfig");
            if (resourceGet == null)
            {
                resourceGet = ScriptableObject.CreateInstance<GplaySDKConfig>();
                AssetDatabase.CreateAsset(resourceGet, "Assets/GplaySDK/Core/Resources/GplaySDKConfig.asset");
            }
        }
    }
}