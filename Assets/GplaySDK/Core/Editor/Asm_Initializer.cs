// Filename: BasicAsmInitialize.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 2:56 PM 23/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.IO;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.Core.Editor.BaseLib;
using GplaySDK.Core.Editor.BaseLib.Attribute;
using GplaySDK.Core.Editor.BaseLib.Utils;
using UnityEngine;

namespace GplaySDK.Core.Editor
{
    internal static class Asm_Initializer
    {
        [EditorInitialize(1000, EditorInitializeLoadType.OnAsmdefInitialize)]
        public static void Initialize()
        {
            //Get asmdef of this class
            const string asmPath = "Assets/GplaySDK/Core/GplaySDK.Core.asmdef";
            string fullPath = Application.dataPath.Replace("Assets", asmPath);
            var asmdefText = File.ReadAllText(fullPath);
            var asmdef = asmdefText.FromJson<AssemblyDefinition>();

            AsmUtils.LibraryInclude(asmdef, "GplaySDK.MediationIntegration.BaseLib");

            AsmUtils.LibraryInclude(asmdef, "GplaySDK.AdmobIntegration");

            AsmUtils.LibraryInclude(asmdef, "GplaySDK.IronSourceIntegration");

            AsmUtils.LibraryInclude(asmdef, "GplaySDK.MaxIntegration");
            
            //Save asmdef after add reference
            var asmdefJson = asmdef.ToJson(true);
            File.WriteAllText(fullPath, asmdefJson);
        }


        
    }
}