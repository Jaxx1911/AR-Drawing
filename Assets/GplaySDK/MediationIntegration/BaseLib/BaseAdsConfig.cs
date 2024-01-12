// Filename: BaseAdsConfig.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:54 PM 09/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GplaySDK.Core.BaseLib;
using GplaySDK.Core.BaseLib.Utils;
using Sirenix.OdinInspector;

namespace GplaySDK.MediationIntegration.BaseLib
{
    public sealed class BaseAdsConfig : BaseConfig<BaseAdsConfig>
    {
        protected override string ConfigName => "AdsConfig";

        protected override BaseAdsConfig _Initialize()
        {
            Instance = this;
            InitializeRemoteConfig();
            return this;
        }

#if UNITY_EDITOR

        public override Task GetConfigFromLocalServer()
        {
            return Task.CompletedTask;
            //Ignore
        }
#endif

        private void InitializeRemoteConfig()
        {
        }

        public static BaseAdsConfig Instance { get; private set; }

        // ReSharper disable once UnassignedField.Global
        [PropertyOrder(0), LabelText("AdBreak Prefab")]
        public IPopup adBreakPrefab;

        #region Ad Decision

        #region Setup

        [TitleGroup("Ad Decision", Order = 1)]
        [FoldoutGroup("Ad Decision/Setup", Order = 0)]
        [LabelText("Banner Ad Decision"), PropertyOrder(0)]
        public List<AdIntegrationType> bannerAdDecision = new List<AdIntegrationType>()
        {
            AdIntegrationType.Admob, AdIntegrationType.Max
        };

        [FoldoutGroup("Ad Decision/Setup")] [LabelText("Interstitial Ad Decision"), PropertyOrder(1)]
        public List<AdIntegrationType> interstitialAdDecision = new List<AdIntegrationType>()
        {
            AdIntegrationType.Max, AdIntegrationType.Admob
        };

        [FoldoutGroup("Ad Decision/Setup")] [LabelText("Rewarded Ad Decision"), PropertyOrder(2)]
        public List<AdIntegrationType> rewardedAdDecision = new List<AdIntegrationType>()
        {
            AdIntegrationType.Max, AdIntegrationType.Admob
        };

        [FoldoutGroup("Ad Decision/Setup")] [LabelText("Open Ads Decision"), PropertyOrder(3)]
        public List<AdIntegrationType> openAdsDecision = new List<AdIntegrationType>()
        {
            AdIntegrationType.Admob
        };

        [FoldoutGroup("Ad Decision/Setup")] [LabelText("Native Ad Decision"), PropertyOrder(4)]
        public List<AdIntegrationType> nativeAdDecision = new List<AdIntegrationType>()
        {
            AdIntegrationType.Admob
        };

        #endregion

        #endregion


        /// <summary>
        /// Event when ad paid
        /// </summary>
        /// <para>AdType</para>
        /// <para>AdIntegration</para>
        /// <para>Amount</para>
        /// <para>Currency</para>
        public static event Action<AdType, string, double, string> OnAdPaid;


        /// <summary>
        /// Only for SDK internal use. Multiple dll can't use default internal method.
        /// </summary>
        public static class Internal
        {
            public static void InvokeAdPaid(AdType adType, string adIntegration, double amount, string currency)
            {
                ThreadUtils.RunOnMainThread(() => OnAdPaid?.Invoke(adType, adIntegration, amount, currency));
            }
        }
    }
}