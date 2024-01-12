// Filename: RuntimeInitializeInfo.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:07 AM 06/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.LoadInfo
{
    [Serializable]
    public class RuntimeInitializeInfo
    {
        public const string FileName = "RuntimeInitializeInfos.inz";
        public string AssemblyName { get; private set; }
        public string TypeName { get; private set; }
        public string MethodName { get; private set; }

        [JsonIgnore] public int Order { get; private set; }


        private RuntimeInitializeInfo()
        {
        }

        public RuntimeInitializeInfo(string assemblyName, string typeName, string methodName, int order)
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
            MethodName = methodName;
            Order = order;
        }

        public void Initialize()
        {
            var method = AppDomain.CurrentDomain.Load(AssemblyName)
                .GetType(TypeName)
                .GetMethod(MethodName, BindingFlags.Static 
                                       | BindingFlags.Public 
                                       | BindingFlags.NonPublic);
            if (method == null)
            {
                throw new System.Exception($"Property {MethodName} not found in type {TypeName}");
            }

#if G_LOG_LOW_LEVEL
            Debug.Log("Runtime Initialized: " + TypeName + "." + MethodName);
#endif
            method.Invoke(null, null);
        }

        [Serializable]
        public class RuntimeInitializeInfoPack
        {
            public RuntimeInitializeInfo[] afterAssembliesLoaded = Array.Empty<RuntimeInitializeInfo>();

            public RuntimeInitializeInfo[] beforeSceneLoad = Array.Empty<RuntimeInitializeInfo>();

            public RuntimeInitializeInfo[] afterSceneLoad = Array.Empty<RuntimeInitializeInfo>();

            private RuntimeInitializeInfoPack()
            {
                
            }

            public RuntimeInitializeInfoPack(RuntimeInitializeInfo[] afterAssembliesLoaded,
                RuntimeInitializeInfo[] beforeSceneLoad,
                RuntimeInitializeInfo[] afterSceneLoad)
            {
                this.afterAssembliesLoaded = afterAssembliesLoaded;
                this.beforeSceneLoad = beforeSceneLoad;
                this.afterSceneLoad = afterSceneLoad;
            }
        }
    }
}