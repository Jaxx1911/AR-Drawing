// Filename: GplayInitializeValidate.cs
// Purpose: To validate GplayInitializeAttribute. All methods with GplayInitializeAttribute must be static.
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:51 AM 22/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Linq;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.Editor.BaseLib.Attribute;
using UnityEditor;
using UnityEditor.Build;

namespace GplaySDK.Core.Editor.BaseLib.Validator
{
    internal static class RuntimeInitializerValidator
    {
        [EditorInitialize(-1000, EditorInitializeLoadType.OnValidate)]
        private static void ValidateRuntimeInitializerAttribute()
        {
            var allMethods = TypeCache.GetMethodsWithAttribute<RuntimeInitializeAttribute>();
            foreach (var method in allMethods.Where(method => !method.IsStatic))
            {
                throw new BuildFailedException("Method " + method.Name + " in class " + method.DeclaringType!.Name +
                                               " is not static. Please check again!");
            }
        }
    }
}