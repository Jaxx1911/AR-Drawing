// Filename: MediationIntegrationInitInfo.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 10:43 26/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using GplaySDK.Core.BaseLib.Utils;

namespace GplaySDK.MediationIntegration.BaseLib.LoadInfo
{
    [Serializable]
    public class MediationIntegrationInitInfo
    {
        public const string FileName = "MediationIntegrationInitInfo.inz";
        
        public string AssemblyName { get; private set; }
        public string TypeName { get; private set; }

        private MediationIntegrationInitInfo()
        {
        }

        public MediationIntegrationInitInfo(string assemblyName, string typeName)
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
        }

        public BaseAdsIntegration Initialize()
        {
            var type = AppDomain.CurrentDomain.Load(AssemblyName)
                .GetType(TypeName);

            if (Activator.CreateInstance(type) is BaseAdsIntegration instance) return instance;
#if LOG_ERROR
                
            $"Cannot create instance of {TypeName}".LogError();
#endif
            return null;
        }

        [Serializable]
        public class MediationIntegrationInitInfoPack
        {
            public MediationIntegrationInitInfo[] allInitInfos = Array.Empty<MediationIntegrationInitInfo>();

            private MediationIntegrationInitInfoPack()
            {
                
            }

            public MediationIntegrationInitInfoPack(MediationIntegrationInitInfo[] allInitInfos)
            {
                this.allInitInfos = allInitInfos;
            }
        }
    }
}