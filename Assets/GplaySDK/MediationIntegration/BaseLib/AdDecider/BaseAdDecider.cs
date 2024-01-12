// Filename: AdDecider.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 14:41 22/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Linq;
using GplaySDK.Core.BaseLib.Utils;
using Sirenix.OdinInspector;

namespace GplaySDK.MediationIntegration.BaseLib.AdDecider
{
#if G_LOG_LOW_LEVEL
    [Serializable]
#endif
    public abstract class BaseAdDecider
    {
        protected readonly List<BaseAdsIntegration> adsIntegrations;

        protected readonly AdType adType;

        [ShowInInspector]
        public int AdIntegrationCount => adsIntegrations.Count;
        
        [ShowInInspector]
        public bool HasAvailableAds => adsIntegrations.Any(IsAdsIntegrationReady);

        protected BaseAdDecider(AdType adType)
        {
            adsIntegrations = new List<BaseAdsIntegration>();
            this.adType = adType;
        }

        private bool IsContainAdsIntegration(BaseAdsIntegration adsIntegration)
        {
            return adsIntegrations.Any(filter =>
                filter.GetAdIntegrationType() == adsIntegration.GetAdIntegrationType());
        }

        public virtual void AddAdsIntegration(BaseAdsIntegration adsIntegration, List<AdIntegrationType> baseOrder)
        {
            if (adsIntegration == null)
            {
#if LOG_VERBOSE
                "Ads integration is null".Log();
#endif
                return;
            }

            if (IsContainAdsIntegration(adsIntegration))
            {
#if LOG_VERBOSE
                "Ads integration is already added".Log();
#endif
                return;
            }

#if LOG_VERBOSE
            $"Add ads integration {adType}: {adsIntegration.GetAdIntegrationType()}".Log();
#endif
            adsIntegrations.Add(adsIntegration);
            adsIntegrations.Sort((a, b) =>
            {
                var aIndex = baseOrder.IndexOf(a.GetAdIntegrationType());
                var bIndex = baseOrder.IndexOf(b.GetAdIntegrationType());
                return aIndex.CompareTo(bIndex);
            });
        }

        protected BaseAdsIntegration GetAdsIntegration()
        {
            return adsIntegrations.FirstOrDefault(IsAdsIntegrationReady);
        }

        protected bool IsAdsIntegrationReady(BaseAdsIntegration adsIntegration)
        {
            switch (adType)
            {
                case AdType.Banner:
                    return adsIntegration.IsLoadedBanner;
                case AdType.Interstitial:
                    return adsIntegration.IsLoadedInterstitial;
                case AdType.Rewarded:
                    return adsIntegration.IsLoadedRewardedVideo;
                case AdType.OpenAds:
                    return adsIntegration.IsLoadedOpenAds;
                case AdType.NativeAds:
                    return adsIntegration.IsLoadedNativeAds;
                case AdType.None:
                default:
#if LOG_VERBOSE
                    "Ad type not support".Log();
#endif
                    return false;
            }
        }
    }
}