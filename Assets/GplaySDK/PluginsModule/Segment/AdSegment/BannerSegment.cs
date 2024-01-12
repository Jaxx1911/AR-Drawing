// Filename: BannerSegment.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 16:23 28/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Linq;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Const;
using GplaySDK.RemoteConfig;
using UnityEngine.SceneManagement;
using RConfig = GplaySDK.RemoteConfig.RemoteConfigController.Firebase;

namespace GplaySDK.Segment
{
    public sealed class BannerSegment : AdSegment
    {
        private string[] _hideOnScenes;

        protected override void FetchConfig()
        {
            minimumLevel =
                RConfig.GetInt(StringConst.RemoteKey.Segment.Banner.MinimumLevel, 2);
            _hideOnScenes =
                RConfig.GetJson(StringConst.RemoteKey.Segment.Banner.HideOnScenes,
                    Array.Empty<string>());
        }
        
        public bool IsShowAds(Scene showScene)
        {
            if (!isEnable) return false;
            if (minimumLevel > CurrentLevelAttribute.Value) return false;
            if (_hideOnScenes.Contains(showScene.name)) return false;
            return true;
        }
        
    }
}