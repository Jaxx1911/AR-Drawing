// Filename: RewardedAdDecider.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:42 27/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.MediationIntegration.BaseLib.AdDecider
{
    public class RewardedAdDecider : BaseAdDecider
    {
        public RewardedAdDecider() : base(AdType.Rewarded)
        {
        }

        public void ShowRewardedVideo(Action onReward, Action onNotReady, Action onClose, string placement)
        {
            GetAdsIntegration().ShowRewardedVideo(onReward, onNotReady, onClose, placement);
        }
    }
}