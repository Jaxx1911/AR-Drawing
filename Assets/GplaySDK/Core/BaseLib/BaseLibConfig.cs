// Filename: BaseLibConfig.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:08 AM 05/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GplaySDK.Core.BaseLib
{
    public class BaseLibConfig : BaseConfig<BaseLibConfig>
    {
        protected override string ConfigName => "BaseLibConfig";

        #region SDK Config

        [SerializeField, LabelText("GplaySDK Version"), PropertyOrder(0)]
        internal string gplaySDK_Version = "0.0.1";

        #region Project Alias Id

        [FoldoutGroup("Project SDK Alias Id", 1)]
        [ShowInInspector, LabelText("Project SDK Alias Id"), PropertyOrder(0)]
        public string projectSdkAliasId
        {
            get
            {
#if UNITY_ANDROID
                return projectSdkAliasIdAndroid;
#elif UNITY_IOS
                return projectSdkAliasIdIOS;
#endif
            }
        }

        [SerializeField, LabelText("Project SDK Alias Id Android"), PropertyOrder(1),
         FoldoutGroup("Project SDK Alias Id")]
        private string projectSdkAliasIdAndroid;

        [SerializeField, LabelText("Project SDK Alias Id IOS"), PropertyOrder(2),
         FoldoutGroup("Project SDK Alias Id")]
        private string projectSdkAliasIdIOS;

        #endregion

        [SerializeField, LabelText("Debug Mode"), PropertyOrder(2)]
        internal bool gplaySDK_DebugMode = false;


        [SerializeField, LabelText("Log Low Level"), PropertyOrder(3)]
        internal bool gplaySDK_LogLowLevel = false;

        [SerializeField, LabelText("Async Interval"), PropertyOrder(4)]
        internal int asyncInterval = 500;

        [SerializeField, LabelText("Thread CatchUp Single Frame Command"), PropertyOrder(5)]
        internal int threadCatchUpSingleFrameCommand = 10;

        [SerializeField, LabelText("Skip RequireProperty"), PropertyOrder(6)]
        internal bool skipRequireProperty = false;

        #region Git Info

        [PropertyOrder(0), FoldoutGroup("Git Info", 7)] [ReadOnly, ShowInInspector]
        public string gitBranch = "master";

        [PropertyOrder(1), FoldoutGroup("Git Info")] [ReadOnly, ShowInInspector]
        public string gitCommitId = "commitId";

        #endregion

        #endregion


        public static BaseLibConfig Instance { get; private set; }

        protected override BaseLibConfig _Initialize()
        {
            Instance = this;
            return this;
        }
#if UNITY_EDITOR
        
        public override async Task GetConfigFromLocalServer()
        {
            //ignore
        }
#endif


#if UNITY_EDITOR
        public void InitializeSymbol(ref string currentSymbols)
        {
            //GplaySDK Low Level Log
            if (gplaySDK_LogLowLevel)
            {
                if (!currentSymbols.Contains("G_LOG_LOW_LEVEL"))
                {
                    currentSymbols += ";G_LOG_LOW_LEVEL";
                }
            }
            else
            {
                currentSymbols = currentSymbols.Replace("G_LOG_LOW_LEVEL", "");
            }

            //GplaySDK Debug Mode
            if (gplaySDK_DebugMode)
            {
                if (!currentSymbols.Contains("G_DEBUG"))
                {
                    currentSymbols += ";G_DEBUG";
                }
            }
            else
            {
                currentSymbols = currentSymbols.Replace("G_DEBUG", "");
            }

            //GplaySDK Skip RequireProperty
            if (skipRequireProperty)
            {
                if (!currentSymbols.Contains("G_SKIP_REQUIRE_PROPERTY"))
                {
                    currentSymbols += ";G_SKIP_REQUIRE_PROPERTY";
                }
            }
            else
            {
                currentSymbols = currentSymbols.Replace("G_SKIP_REQUIRE_PROPERTY", "");
            }
        }
#endif
    }
}