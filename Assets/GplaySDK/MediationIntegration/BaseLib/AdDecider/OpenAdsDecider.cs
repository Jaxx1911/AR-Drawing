// Filename: OpenAdsDecider.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:53 27/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.MediationIntegration.BaseLib.AdDecider
{
    public class OpenAdsDecider : BaseAdDecider
    {
        public OpenAdsDecider() : base(AdType.OpenAds)
        {
        }

        public void ShowOpenAds(Action onClose, string placement)
        {
            GetAdsIntegration().ShowOpenAds(onClose, placement);
        }
    }
}