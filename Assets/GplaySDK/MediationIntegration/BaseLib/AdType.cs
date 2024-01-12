// Filename: AdType.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:46 AM 02/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

namespace GplaySDK.MediationIntegration.BaseLib
{
    public enum AdType
    {
        None = 0,
        Banner = 1,
        Interstitial = 2,
        Rewarded = 3,
        OpenAds = 4,
        NativeAds = 5,
    }

    public static class AdTypeExtension
    {
        public static string ToLogString(this AdType adType)
        {
            switch (adType)
            {
                case AdType.Banner:
                    return "banner";
                case AdType.Interstitial:
                    return "interstitial";
                case AdType.Rewarded:
                    return "reward";
                case AdType.OpenAds:
                    return "open_ads";
                default:
                    return "none";
            }
        }
    }
}