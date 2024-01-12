// Filename: BasicSymbolInitialize.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 2:47 PM 23/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Linq;
using System.Text.RegularExpressions;
using GplaySDK.Core.Editor.BaseLib.Attribute;
using UnityEditor;
using UnityEditor.Compilation;

namespace GplaySDK.Core.Editor.BaseLib
{
    internal static class BasicSymbolInitialize
    {
        [EditorInitialize(-950, EditorInitializeLoadType.OnSymbolInitialize)]

        private static void Initialize()
        {
            var currentTarget = EditorUserBuildSettings.activeBuildTarget;
            var currentGroup = BuildPipeline.GetBuildTargetGroup(currentTarget);
            string allSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentGroup);
            string[] symbols = allSymbols.Split(';');
            bool isChange = false;
            var allAssembly = CompilationPipeline.GetAssemblies(AssembliesType.PlayerWithoutTestAssemblies)
                .Where(filter => filter.name.StartsWith("GplaySDK"));
            var allAssemblyName = allAssembly as Assembly[] ?? allAssembly.ToArray();
            foreach (var assembly in allAssemblyName)
            {
                var asmSymbols = assembly.name.Replace(".", "_");
                if (!symbols.Contains(asmSymbols))
                {
                    allSymbols += ";" + asmSymbols;
                    symbols = allSymbols.Split(';');
                    isChange = true;
                }
            }

            //Delete all Symbols start with GplaySDK but not in allAssembly
            foreach (var symbol in symbols)
            {
                if (!symbol.StartsWith("GplaySDK") ||
                    allAssemblyName.Any(assembly => assembly.name.Replace(".", "_") == symbol)) continue;
                allSymbols = allSymbols.Replace(symbol, "");
                isChange = true;
            }

            allSymbols = Regex.Replace(allSymbols, @";+", ";");


            if (isChange)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currentGroup, allSymbols);
            }
        }
    }
}