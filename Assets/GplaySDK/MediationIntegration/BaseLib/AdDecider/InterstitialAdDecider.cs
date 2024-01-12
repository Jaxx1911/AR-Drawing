// Filename: InterstitialAdDecider.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:37 27/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.MediationIntegration.BaseLib.AdDecider
{
    public class InterstitialAdDecider: BaseAdDecider
    {
        public InterstitialAdDecider() : base(AdType.Interstitial)
        {
        }


        public void ShowInterstitial(Action onClose, string placement)
        {
            
            GetAdsIntegration().ShowInterstitial(onClose, placement);
        }
    }
}