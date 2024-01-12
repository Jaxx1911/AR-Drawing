// Filename: CooldownInterstitial.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 13:59 29/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using GplaySDK.Core.BaseLib.Const;
using GplaySDK.Segment.PlacementType;
using FRemote = GplaySDK.RemoteConfig.RemoteConfigController.Firebase;

namespace GplaySDK.Segment.BaseAdDeciderLogic
{
    public class CooldownInterstitial : AdDeciderLogic<InterstitialPlacementType, InterstitialSegment>
    {
        private static int _cooldown;
        private static DateTime _nextReadyTime;

        public override int Order => 1;
        public override bool IsRequirement => true;

        public override InterstitialPlacementType placementType => InterstitialPlacementType.Any;

        protected override void Initialize()
        {
            base.Initialize();
            _nextReadyTime = DateTime.MinValue;
        }

        public override bool IsShowAds()
        {
            return DateTime.Now >= _nextReadyTime;
        }

        public override void FetchConfig()
        {
            _cooldown = FRemote.GetInt(StringConst.RemoteKey.Segment.Interstitial.Cooldown, 60);
        }

        public static void ShowInterstitial()
        {
            _nextReadyTime = DateTime.Now.AddSeconds(_cooldown);
        }
    }
}