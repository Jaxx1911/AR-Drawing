// Filename: AsmUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:36 PM 13/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GplaySDK.Core.Editor.BaseLib.Utils
{
    public static class AsmUtils
    {
        public static IEnumerable<Type> GetInheritedOfType(Type abstractType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(abstractType) && !type.IsAbstract);
        }

        public static bool IsLibraryInstalled(string libraryName)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Any(filter => filter.GetName().Name == libraryName);
        }

        public static void LibraryInclude(AssemblyDefinition asmdef, string libName)
        {
            if (IsLibraryInstalled(libName))
            {
                if (asmdef.references.Contains(libName)) return;
                asmdef.references.Add(libName);
                Debug.Log($"Add reference {libName}");
            }
            else
            {
                if (!asmdef.references.Contains(libName)) return;
                asmdef.references.Remove(libName);
                Debug.Log($"Remove reference {libName}");
            }
        }
    }
}