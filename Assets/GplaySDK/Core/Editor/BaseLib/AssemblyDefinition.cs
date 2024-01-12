// Filename: AssemblyDefine.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 12:02 PM 23/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.IO;
using GplaySDK.Core.BaseLib.Utils;
using Newtonsoft.Json;

namespace GplaySDK.Core.Editor.BaseLib
{
    [Serializable]
    public class AssemblyDefinition
    {
        public string name;
        public string rootNamespace;
        public List<string> references;
        public List<string> includePlatforms;
        public List<string> excludePlatforms;
        public bool allowUnsafeCode;
        public bool overrideReferences;
        public List<string> precompiledReferences;
        public bool autoReferenced;
        public List<string> defineConstraints;
        public List<string> versionDefines;
        public bool noEngineReferences;

        public static AssemblyDefinition FromPath(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var allText = File.ReadAllText(path);
            try
            {
                var rt = allText.FromJson<AssemblyDefinition>();
                return rt;
            }
            catch (Exception _)
            {
                
                return null;
            }
        }

        public void SaveToPath(string path)
        {
            File.WriteAllText(path,this.ToJson(true));
        }
    }
}