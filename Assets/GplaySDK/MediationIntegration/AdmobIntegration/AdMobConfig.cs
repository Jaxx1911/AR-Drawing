// Filename: AdMobConfig.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:54 AM 25/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Threading.Tasks;
using GplaySDK.Core.BaseLib;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GplaySDK.AdmobIntegration
{
    public class AdMobConfig : BaseConfig<AdMobConfig>
    {
        protected override string ConfigName => "AdMobConfig";

        protected override AdMobConfig _Initialize()
        {
            Instance = this;
            return this;
        }

#if UNITY_EDITOR
        
        public override async Task GetConfigFromLocalServer()
        {
            var onlineConfig = await LocalServer.LocalServerController.GetAdmobConfig();
            if (onlineConfig == null) return;
            if (onlineConfig.IsAndroid())
            {
                appId_Android = onlineConfig.admobAppId;
                auto_OpenAppIdAndroid = onlineConfig.admobOpenAppId;
                auto_NativeAdsIdAndroid = onlineConfig.admobNativeId;
                auto_BannerAdsIdAndroid = onlineConfig.admobBannerId;
                auto_InterstitialAdsIdAndroid = onlineConfig.admobInterstitialId;
                auto_RewardedVideoAdsIdAndroid = onlineConfig.admobRewardedId;
            }
            else if (onlineConfig.IsIos())
            {
                appId_IOS = onlineConfig.admobAppId;
                auto_OpenAppIdIOS = onlineConfig.admobOpenAppId;
                auto_NativeAdsIdIOS = onlineConfig.admobNativeId;
                auto_BannerAdsIdIOS = onlineConfig.admobBannerId;
                auto_InterstitialAdsIdIOS = onlineConfig.admobInterstitialId;
                auto_RewardedVideoAdsIdIOS = onlineConfig.admobRewardedId;
            }
        }
#endif

        internal static AdMobConfig Instance { get; private set; }

        #region Admob App Id

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Admob App Id", 0)]
        public string AppId
        {
            get
            {
#if UNITY_ANDROID
                return appId_Android;
#elif UNITY_IOS
                return appId_IOS;
#endif
            }
        }

        [SerializeField, LabelText("App Id Android"), PropertyOrder(1),
         FoldoutGroup("Admob App Id")]
        private string appId_Android;

        [SerializeField, LabelText("App Id IOS"), PropertyOrder(2),
         FoldoutGroup("Admob App Id")]
        private string appId_IOS;

        #endregion

        #region Auto Bid| Open App Id

        [TitleGroup("Auto Bid", order: 1)]
        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Auto Bid/Open App Id", 0)]
        public string Auto_OpenAppId
        {
            get
            {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                return "ca-app-pub-3940256099942544/9257395921";

#elif UNITY_ANDROID
                return auto_OpenAppIdAndroid;
#elif UNITY_IOS
                return auto_OpenAppIdIOS;
#endif
            }
        }

        [SerializeField, LabelText("Open App Id Android"), PropertyOrder(1),
         FoldoutGroup("Auto Bid/Open App Id")]
        private string auto_OpenAppIdAndroid;

        [SerializeField, LabelText("Open App Id IOS"), PropertyOrder(2),
         FoldoutGroup("Auto Bid/Open App Id")]
        private string auto_OpenAppIdIOS;

        #endregion

        #region Auto Bid| Native Ads Id

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Auto Bid/Native Ads Id", 1)]
        public string Auto_NativeAdsId
        {
            get
            {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                return "ca-app-pub-3940256099942544/2247696110";

#elif UNITY_ANDROID
                return auto_NativeAdsIdAndroid;
#elif UNITY_IOS
                return auto_NativeAdsIdIOS;
#endif
            }
        }

        [SerializeField, LabelText("Native Ads Id Android"), PropertyOrder(1),
         FoldoutGroup("Auto Bid/Native Ads Id")]
        private string auto_NativeAdsIdAndroid;

        [SerializeField, LabelText("Native Ads Id IOS"), PropertyOrder(2),
         FoldoutGroup("Auto Bid/Native Ads Id")]
        private string auto_NativeAdsIdIOS;

        #endregion

        #region Auto Bid| Banner Ads Id

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Auto Bid/Banner Ads Id", 2)]
        public string Auto_BannerAdsId
        {
            get
            {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                return "ca-app-pub-3940256099942544/6300978111";

#elif UNITY_ANDROID
                return auto_BannerAdsIdAndroid;
#elif UNITY_IOS
                return auto_BannerAdsIdIOS;
#endif
            }
        }

        [SerializeField, LabelText("Banner Ads Id Android"), PropertyOrder(1),
         FoldoutGroup("Auto Bid/Banner Ads Id")]
        private string auto_BannerAdsIdAndroid;

        [SerializeField, LabelText("Banner Ads Id IOS"), PropertyOrder(2),
         FoldoutGroup("Auto Bid/Banner Ads Id")]
        private string auto_BannerAdsIdIOS;

        #endregion

        #region Auto Bid| Interstitial Ads Id

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Auto Bid/Interstitial Ads Id", 3)]
        public string Auto_InterstitialAdsId
        {
            get
            {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                return "ca-app-pub-3940256099942544/1033173712";

#elif UNITY_ANDROID
                return auto_InterstitialAdsIdAndroid;
#elif UNITY_IOS
                return auto_InterstitialAdsIdIOS;
#endif
            }
        }

        [SerializeField, LabelText("Interstitial Ads Id Android"), PropertyOrder(1),
         FoldoutGroup("Auto Bid/Interstitial Ads Id")]
        private string auto_InterstitialAdsIdAndroid;

        [SerializeField, LabelText("Interstitial Ads Id IOS"), PropertyOrder(2),
         FoldoutGroup("Auto Bid/Interstitial Ads Id")]
        private string auto_InterstitialAdsIdIOS;

        #endregion

        #region Auto Bid| Rewarded Video Ads Id

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Auto Bid/Rewarded Video Ads Id", 4)]
        public string Auto_RewardedVideoAdsId
        {
            get
            {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                return "ca-app-pub-3940256099942544/5224354917";

#elif UNITY_ANDROID
                return auto_RewardedVideoAdsIdAndroid;
#elif UNITY_IOS
                return auto_RewardedVideoAdsIdIOS;
#endif
            }
        }

        [SerializeField, LabelText("Rewarded Video Ads Id Android"), PropertyOrder(1),
         FoldoutGroup("Auto Bid/Rewarded Video Ads Id")]
        private string auto_RewardedVideoAdsIdAndroid;

        [SerializeField, LabelText("Rewarded Video Ads Id IOS"), PropertyOrder(2),
         FoldoutGroup("Auto Bid/Rewarded Video Ads Id")]
        private string auto_RewardedVideoAdsIdIOS;

        #endregion

        #region Ads Config

        [BoxGroup("Ads Config", order: 6), LabelText("Use Collapsed Banner"), PropertyOrder(0)]
        public bool useCollapsedBanner = true;

        [BoxGroup("Ads Config"), LabelText("Use as Sub Mediation"), PropertyOrder(1)]
        public bool useAsSubMediation = true;

        #endregion
    }
}