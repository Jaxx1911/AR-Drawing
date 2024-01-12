// Filename: InterstitialSegment.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 16:40 28/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Collections.Generic;
using GplaySDK.Core.BaseLib.Const;
using GplaySDK.RemoteConfig;
using GplaySDK.Segment.PlacementType;

using RConfig = GplaySDK.RemoteConfig.RemoteConfigController.Firebase;

namespace GplaySDK.Segment
{
    public sealed class InterstitialSegment : AdSegment
    {
        private readonly Dictionary<InterstitialPlacementType,
            AdDecider<InterstitialPlacementType, InterstitialSegment>> _adCheckDict =
            new Dictionary<InterstitialPlacementType, AdDecider<InterstitialPlacementType, InterstitialSegment>>();

        protected override void FetchConfig()
        {
            minimumLevel =
                RConfig.GetInt(StringConst.RemoteKey.Segment.Interstitial.MinimumLevel, 2);
            
            // Fetch All AdDecider
            foreach (var adDecider in _adCheckDict.Values)
            {
                adDecider.FetchConfig();
            }
        }

        public bool IsShowAds(InterstitialPlacementType placementType)
        {
            var anyCondition = _adCheckDict[InterstitialPlacementType.Any]?.IsShowAds() ?? true;
            var placementCondition = _adCheckDict[placementType]?.IsShowAds() ?? true;
            return anyCondition && placementCondition;
        }
    }
}