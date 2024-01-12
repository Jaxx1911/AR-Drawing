// Filename: MaxConfig.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:45 PM 03/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Threading.Tasks;
using GplaySDK.Core.BaseLib;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GplaySDK.MaxIntegration
{
    public class MaxConfig : BaseConfig<MaxConfig>
    {
        protected override string ConfigName => "MaxConfig";

        protected override MaxConfig _Initialize()
        {
            Instance = this;
            return this;
        }

#if UNITY_EDITOR
        
        public override async Task GetConfigFromLocalServer()
        {
            var onlineConfig = await LocalServer.LocalServerController.GetMaxSdkConfig();
            if (onlineConfig == null) return;
            if (onlineConfig.IsAndroid())
            {
                sdkKeyAndroid = onlineConfig.maxSdkKey;
                bannerAdUnitIdAndroid = onlineConfig.maxSdkBannerId;
                interstitialAdUnitIdAndroid = onlineConfig.maxSdkInterstitialId;
                rewardedAdUnitIdAndroid = onlineConfig.maxSdkRewardedId;
            }
            else if (onlineConfig.IsIos())
            {
                sdkKeyIOS = onlineConfig.maxSdkKey;
                bannerAdUnitIdIOS = onlineConfig.maxSdkBannerId;
                interstitialAdUnitIdIOS = onlineConfig.maxSdkInterstitialId;
                rewardedAdUnitIdIOS = onlineConfig.maxSdkRewardedId;
            }
        }
#endif

        internal static MaxConfig Instance { get; private set; }

        #region SDK Key

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("SDK Key")]
        public string SDKKey
        {
            get
            {
#if UNITY_ANDROID
                return sdkKeyAndroid;
#elif UNITY_IOS
                return sdkKeyIOS;
#endif
            }
        }

        [SerializeField, LabelText("SDK Key Android"), PropertyOrder(1), FoldoutGroup("SDK Key")]
        private string sdkKeyAndroid;

        [SerializeField, LabelText("SDK Key IOS"), PropertyOrder(2), FoldoutGroup("SDK Key")]
        private string sdkKeyIOS;

        #endregion

        #region Banner Ad Unit Id

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Banner Ad Unit Id")]
        public string BannerAdUnitId
        {
            get
            {
#if UNITY_ANDROID
                return bannerAdUnitIdAndroid;
#elif UNITY_IOS
                return bannerAdUnitIdIOS;
#endif
            }
        }

        [SerializeField, LabelText("Banner Ad Unit Id Android"), PropertyOrder(1), FoldoutGroup("Banner Ad Unit Id")]
        private string bannerAdUnitIdAndroid;

        [SerializeField, LabelText("Banner Ad Unit Id IOS"), PropertyOrder(2), FoldoutGroup("Banner Ad Unit Id")]
        private string bannerAdUnitIdIOS;

        #endregion

        #region Interstitial Ad Unit Id

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Interstitial Ad Unit Id")]
        public string InterstitialAdUnitId
        {
            get
            {
#if UNITY_ANDROID
                return interstitialAdUnitIdAndroid;
#elif UNITY_IOS
                return interstitialAdUnitIdIOS;
#endif
            }
        }

        [SerializeField, LabelText("Interstitial Ad Unit Id Android"), PropertyOrder(1),
         FoldoutGroup("Interstitial Ad Unit Id")]
        private string interstitialAdUnitIdAndroid;

        [SerializeField, LabelText("Interstitial Ad Unit Id IOS"), PropertyOrder(2),
         FoldoutGroup("Interstitial Ad Unit Id")]
        private string interstitialAdUnitIdIOS;

        #endregion

        #region Rewarded Ad Unit Id

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Rewarded Ad Unit Id")]
        public string RewardedAdUnitId
        {
            get
            {
#if UNITY_ANDROID
                return rewardedAdUnitIdAndroid;
#elif UNITY_IOS
                return rewardedAdUnitIdIOS;
#endif
            }
        }

        [SerializeField, LabelText("Rewarded Ad Unit Id Android"), PropertyOrder(1),
         FoldoutGroup("Rewarded Ad Unit Id")]
        private string rewardedAdUnitIdAndroid;

        [SerializeField, LabelText("Rewarded Ad Unit Id IOS"), PropertyOrder(2), FoldoutGroup("Rewarded Ad Unit Id")]
        private string rewardedAdUnitIdIOS;

        #endregion

        #region Debug Config

        [SerializeField, LabelText("Enable Init Debug Mode"), PropertyOrder(2), FoldoutGroup("Debug Config")]
        internal bool enableInitDebugMode = false;

        #endregion
    }
}