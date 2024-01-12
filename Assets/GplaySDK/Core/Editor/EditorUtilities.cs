// Filename: EditorUtilities.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 2:48 PM 02/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using UnityEditor;

namespace GplaySDK.Core.Editor
{
    internal static class EditorUtilities
    {
        [MenuItem("LA/Config", priority = 1)]
        private static void OpenConfig()
        {
            var config = GplaySDKConfig.GetConfig();
            Selection.activeObject = config;
        }
    }
}