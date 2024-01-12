// Filename: Editor_Initializer.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 1:50 PM 05/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Linq;
using GplaySDK.Core.Editor.BaseLib.Attribute;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace GplaySDK.Core.Editor.BaseLib
{
    public static class Editor_Initializer
    {
        [MenuItem("LA/Initializer/Initialize All", priority = 0)]
        public static void InitializeAll()
        {
            Validate();
            InitializeAsmdef();
            InitializeSymbols();
            InitializeCompile();
            AssetDatabase.Refresh();
        }

        [MenuItem("LA/Initializer/Validate", priority = 1)]
        public static void Validate()
        {
            var allAsmInitializer = TypeCache
                .GetMethodsWithAttribute<EditorInitializeAttribute>()
                .Where(filter =>
                    filter.GetAttribute<EditorInitializeAttribute>()
                        .LoadType == EditorInitializeLoadType.OnValidate)
                .ToArray();
            allAsmInitializer.Sort((a, b) =>
            {
                var orderA = a.GetAttribute<EditorInitializeAttribute>().Order;
                var orderB = b.GetAttribute<EditorInitializeAttribute>().Order;
                return orderA.CompareTo(orderB);
            });
            foreach (var initializer in allAsmInitializer)
            {
                initializer.Invoke(null, null);
            }
        }


        [MenuItem("LA/Initializer/Initialize Asmdef", priority = 2)]
        public static void InitializeAsmdef()
        {
            var allAsmInitializer = TypeCache
                .GetMethodsWithAttribute<EditorInitializeAttribute>()
                .Where(filter =>
                    filter.GetAttribute<EditorInitializeAttribute>()
                        .LoadType == EditorInitializeLoadType.OnAsmdefInitialize)
                .ToArray();
            allAsmInitializer.Sort((a, b) =>
            {
                var orderA = a.GetAttribute<EditorInitializeAttribute>().Order;
                var orderB = b.GetAttribute<EditorInitializeAttribute>().Order;
                return orderA.CompareTo(orderB);
            });
            foreach (var initializer in allAsmInitializer)
            {
                initializer.Invoke(null, null);
            }
        }

        [MenuItem("LA/Initializer/Initialize Symbol", priority = 3)]
        public static void InitializeSymbols()
        {
            var allAsmInitializer = TypeCache
                .GetMethodsWithAttribute<EditorInitializeAttribute>()
                .Where(filter =>
                    filter.GetAttribute<EditorInitializeAttribute>()
                        .LoadType == EditorInitializeLoadType.OnSymbolInitialize)
                .ToArray();
            allAsmInitializer.Sort((a, b) =>
            {
                var orderA = a.GetAttribute<EditorInitializeAttribute>().Order;
                var orderB = b.GetAttribute<EditorInitializeAttribute>().Order;
                return orderA.CompareTo(orderB);
            });
            foreach (var initializer in allAsmInitializer)
            {
                initializer.Invoke(null, null);
            }
        }

        [MenuItem("LA/Initializer/Initialize Compile", priority = 4)]
        public static void InitializeCompile()
        {
            var allAsmInitializer = TypeCache
                .GetMethodsWithAttribute<EditorInitializeAttribute>()
                .Where(filter =>
                    filter.GetAttribute<EditorInitializeAttribute>()
                        .LoadType == EditorInitializeLoadType.OnCompileInitialize)
                .ToArray();
            allAsmInitializer.Sort((a, b) =>
            {
                var orderA = a.GetAttribute<EditorInitializeAttribute>().Order;
                var orderB = b.GetAttribute<EditorInitializeAttribute>().Order;
                return orderA.CompareTo(orderB);
            });
            foreach (var initializer in allAsmInitializer)
            {
                initializer.Invoke(null, null);
            }
        }
        [MenuItem("LA/Initializer/Fix Compile Define", priority = 5)]
        private static void FixCompileDefine()
        {
            var currentTarget = EditorUserBuildSettings.activeBuildTarget;
            var currentGroup = BuildPipeline.GetBuildTargetGroup(currentTarget);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentGroup, "");
            AssetDatabase.Refresh();
            InitializeAll();
        }
    }
}