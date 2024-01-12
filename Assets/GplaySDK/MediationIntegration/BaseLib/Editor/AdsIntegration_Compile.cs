// Filename: AdsIntegration_Compile.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:54 25/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Collections.Generic;
using System.Linq;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.Core.Editor.BaseLib.Attribute;
using GplaySDK.Core.Editor.BaseLib.Utils;
using GplaySDK.MediationIntegration.BaseLib.LoadInfo;
using UnityEditor;
using UnityEngine;

namespace GplaySDK.MediationIntegration.BaseLib.Editor
{
    internal static class AdsIntegration_Compile
    {
        [MenuItem("LA/Test Script")]
        [EditorInitialize(15, EditorInitializeLoadType.OnCompileInitialize)]
        private static void Compile()
        {
            var allInherited = AsmUtils.GetInheritedOfType(typeof(BaseAdsIntegration)).ToList();
            var saveInfo = new List<MediationIntegrationInitInfo>();
            foreach (var inherited in allInherited)
            {
                saveInfo.Add(new MediationIntegrationInitInfo(inherited.Assembly.GetName().Name, inherited.FullName));
            }

            saveInfo.Log();

            StreamingAssetUtils.SaveToStreamingAsset(MediationIntegrationInitInfo.FileName,
                new MediationIntegrationInitInfo.MediationIntegrationInitInfoPack(saveInfo.ToArray()).ToJson());
        }
    }
}