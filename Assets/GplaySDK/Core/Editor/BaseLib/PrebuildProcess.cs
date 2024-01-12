// Filename: BuildUtils.cs
// Purpose: Define all asm symbols for GplaySDK
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:33 PM 21/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace GplaySDK.Core.Editor.BaseLib
{
    [InitializeOnLoad]
    internal class PrebuildProcess: IPreprocessBuildWithReport
    {
        public int callbackOrder => -1;

        static PrebuildProcess()
        {
            
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            Editor_Initializer.InitializeAll();
        }
    }
}