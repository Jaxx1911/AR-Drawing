// Filename: BannerDecider.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 15:35 26/12/2023
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
    public sealed class BannerDecider : BaseAdDecider
    {
        [ShowInInspector]
        private bool _isBannerShowing = false;

        public BannerDecider() : base(AdType.Banner)
        {
        }

        public override void AddAdsIntegration(BaseAdsIntegration adsIntegration, List<AdIntegrationType> baseOrder)
        {
            base.AddAdsIntegration(adsIntegration, baseOrder);
            if (!_isBannerShowing) return;
#if LOG_VERBOSE
            "Banner is showing, try to show higher priority banner".Log();
#endif
            ShowBanner();
        }

        public void OnBannerUpdated()
        {
            if (!_isBannerShowing) return;
#if LOG_VERBOSE
            "Banner is showing, try to show higher priority banner".Log();
#endif
            ShowBanner();
        }

        public void ShowBanner()
        {
            var highestAvailable = GetAdsIntegration();
            _isBannerShowing = true;
            if (highestAvailable == null) return;
#if LOG_VERBOSE
            $"Show highest priority banner: {highestAvailable.GetAdIntegrationType()}".Log();
#endif
            highestAvailable.ShowBanner();
            foreach (var integration in adsIntegrations.Where(integration => integration != highestAvailable))
                integration.HideBanner();
        }

        public void HideBanner()
        {
            foreach (var integration in adsIntegrations)
            {
                integration.HideBanner();
            }

            _isBannerShowing = false;
        }
    }
}