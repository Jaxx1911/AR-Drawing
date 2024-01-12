// Filename: BaseAdsIntegration.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:09 AM 25/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using Firebase.Analytics;
using GplaySDK.BaseLib;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Utils;
using UnityEngine;

// ReSharper disable once CheckNamespace Skip only for this file
namespace GplaySDK.MediationIntegration.BaseLib
{
    public abstract class BaseAdsIntegration
    {
        public abstract AdIntegrationType IntegrationType { get; }
        
        public event Action OnBannerUpdated;
        

        protected string IntegrationName => IntegrationType.ToString();
            
        public abstract bool IsLoadedBanner { get; }

        public abstract bool IsLoadedInterstitial { get; }

        public abstract bool IsLoadedRewardedVideo { get; }

        public abstract bool IsLoadedOpenAds { get; }

        public abstract bool IsLoadedNativeAds { get; }

        protected bool onLoadingInterstitial = false;
        protected bool onLoadingRewardedVideo = false;
        protected bool onLoadingOpenAds = false;

        protected bool onInitializing = false;


        protected bool isInitialized = false;
        
        public void Initialize(Action onSuccess = null)
        {
            if (isInitialized)
            {
                onSuccess?.Invoke();
                return;
            }
            _Initialize(onSuccess);
        }

        protected abstract void _Initialize(Action onSuccess = null);


        protected abstract void _ShowBanner();

        public void ShowBanner()
        {
            if (!isInitialized)
            {
                _Initialize();
                return;
            }

            _ShowBanner();
        }

        protected abstract void _HideBanner();

        public void HideBanner()
        {
            if (!isInitialized)
            {
                _Initialize();
                return;
            }

            _HideBanner();
        }

        protected abstract void _LoadInterstitial();

        public void LoadInterstitial()
        {
            if (!isInitialized)
            {
                _Initialize();
                return;
            }

            if (onLoadingInterstitial)
            {
#if LOG_ERROR
                Debug.LogError($"{IntegrationType} Already loading interstitial");
#endif
                return;
            }

            if (IsLoadedInterstitial)
            {
#if LOG_VERBOSE
                Debug.Log($"{IntegrationType} Interstitial is loaded");
#endif
                return;
            }

            _LoadInterstitial();
        }

        protected abstract void _ShowInterstitial(Action onClose, string placement);

        public void ShowInterstitial(Action onClose, string placement)
        {
            if (!isInitialized)
            {
                onClose?.Invoke();
                _Initialize();
                return;
            }

            if (!IsLoadedInterstitial)
            {
                onClose?.Invoke();
                _LoadInterstitial();
                return;
            }

            _ShowInterstitial(onClose, placement);
            AdsDatabase.NumberAdsToday++;
            AdsDatabase.NumberAdsThisSession++;
        }

        protected abstract void _LoadRewardedVideo();

        public void LoadRewardedVideo()
        {
            if (!isInitialized)
            {
                _Initialize();
                return;
            }

            if (onLoadingRewardedVideo)
            {
#if LOG_ERROR
                Debug.LogError($"{IntegrationType} Already loading rewarded video");
#endif
                return;
            }

            if (IsLoadedRewardedVideo)
            {
#if LOG_VERBOSE
                Debug.Log($"{IntegrationType} Rewarded video is loaded");
#endif
                return;
            }

            _LoadRewardedVideo();
        }


        protected abstract void _ShowRewardedVideo(Action actionReward,
            Action actionOnFail,
            Action actionClose,
            string placement);

        public void ShowRewardedVideo(
            Action actionReward,
            Action actionOnFail,
            Action actionClose,
            string placement)
        {
            if (!isInitialized)
            {
                actionOnFail?.Invoke();
                _Initialize();
                return;
            }

            if (!IsLoadedRewardedVideo)
            {
                actionOnFail?.Invoke();
                _LoadRewardedVideo();
                return;
            }


            _ShowRewardedVideo(actionReward, actionOnFail, actionClose, placement);
        }

        protected abstract void _LoadOpenAds(Action onSuccess);

        public void LoadOpenAds(Action onSuccess)

        {
            if (!isInitialized)
            {
                _Initialize();
                return;
            }

            if (onLoadingOpenAds)
            {
#if LOG_ERROR
                Debug.LogError($"{IntegrationType} Already loading open ads");
#endif
                return;
            }

            if (IsLoadedOpenAds)
            {
#if LOG_VERBOSE
                Debug.Log($"{IntegrationType} Open ads is loaded");
#endif
                return;
            }

            _LoadOpenAds(onSuccess);
        }

        protected abstract void _ShowOpenAds(string placementName, Action actionClose);

        public void ShowOpenAds(Action actionClose, string placementName)
        {
            if (!isInitialized)
            {
                _Initialize();
                return;
            }

            if (!IsLoadedOpenAds)
            {
                LoadOpenAds(() => ShowOpenAds(actionClose, placementName));
                return;
            }


            _ShowOpenAds(placementName, actionClose);
        }

        protected abstract void _LoadNativeAds(Action onSuccess);

        public void LoadNativeAds(Action onSuccess)
        {
            if (!isInitialized)
            {
                _Initialize();
                return;
            }

            if (IsLoadedNativeAds)
            {
#if LOG_VERBOSE
                Debug.Log($"{IntegrationType} Native ads is loaded");
#endif
                return;
            }

            _LoadNativeAds(onSuccess);
        }

        protected abstract void _ShowNativeAds(string placementName, Action actionClose);

        public void ShowNativeAds(string placementName, Action actionClose)
        {
            if (!isInitialized)
            {
                _Initialize();
                return;
            }

            if (!IsLoadedNativeAds)
            {
                LoadNativeAds(() => ShowNativeAds(placementName, actionClose));
                return;
            }

            _ShowNativeAds(placementName, actionClose);
        }

        internal AdIntegrationType GetAdIntegrationType()
        {
            return IntegrationType;
        }


        protected void LogAdRevenue(AdType adType, string adPlatform, double value,
            string currency, bool isSubRev)
        {
            //Prevent call from sub thread an can't use Unity Function
            ThreadUtils.RunOnMainThread(Handle);

            void Handle()
            {
                List<Parameter> parameters = new List<Parameter>()
                {
                    new Parameter("ad_platform", adPlatform),
                    new Parameter("ad_format", adType.ToLogString()),
                    new Parameter("value", value),
                    new Parameter("currency", currency)
                };
                FirebaseAnalytics.LogEvent("ad_impression", parameters.ToArray());
#if GplaySDK_CostCenter
                if (isSubRev) FirebaseAnalytics.LogEvent("sub_revenue", parameters.ToArray());
                var level = CurrentLevelAttribute.Value;
                var levelMode = CurrentLevelModeAttribute.Value;
                parameters.Add(new Parameter("level", level));
                parameters.Add(new Parameter("level_mode", levelMode));
                FirebaseAnalytics.LogEvent("ad_revenue_sdk", parameters.ToArray());
#endif
            }
        }

        protected void LogAdView(AdType adType, string adPlatform, string placement)
        {
            //Prevent call from sub thread an can't use Unity Function
            ThreadUtils.RunOnMainThread(Handle);

            void Handle()
            {
                List<Parameter> parameters = new List<Parameter>()
                {
                    new Parameter("ad_platform", adPlatform),
                    new Parameter("ad_format", adType.ToLogString()),
                    new Parameter("placement", placement),
                    new Parameter("device_id", DeviceIdAttribute.Value),
                };
                FirebaseAnalytics.LogEvent("ad_view", parameters.ToArray());
            }
        }

        protected void InvokeBannerUpdated()
        {
#if LOG_VERBOSE
            $"OnBannerUpdated {IntegrationName}".Log();
#endif
            OnBannerUpdated?.Invoke();
        }
    }
}