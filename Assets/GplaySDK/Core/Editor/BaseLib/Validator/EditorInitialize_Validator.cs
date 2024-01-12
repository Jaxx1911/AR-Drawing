// Filename: EditorInitialize_Validator.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 1:56 PM 05/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Linq;
using GplaySDK.Core.Editor.BaseLib.Attribute;
using UnityEditor;
using UnityEditor.Build;

namespace GplaySDK.Core.Editor.BaseLib.Validator
{
    public static class EditorInitialize_Validator
    {
        [EditorInitialize(-10000, EditorInitializeLoadType.OnValidate)]
        private static void Validator()
        {
            var allMethods = TypeCache.GetMethodsWithAttribute<EditorInitializeAttribute>();
            foreach (var method in allMethods.Where(method => !method.IsStatic))
            {
                throw new BuildFailedException("Method " + method.Name + " in class " + method.DeclaringType!.Name +
                                               " is not static. Please check again!");
            }
        }
    }
}