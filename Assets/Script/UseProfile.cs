using System.Collections;
using System.Collections.Generic;
using GplaySDK.BaseLib.RequireProperty;
using GplaySDK.Core.BaseLib.Attribute;
using Script;
using UnityEngine;


namespace DefaultNamespace
{
    public static class UseProfile
    {
        [MinimumLevelShowInterstitial]
        public static int MinLevelShowInterAds => 2;

        [InterstitialCooldown] public static int InterstitialCD => 60;

        [GdprValue]
        public static bool GdprValue
        {
            get => PlayerPrefs.GetInt(StringConst.LocalKey.AttributeValue.GDPR_VALUE, 0) != 0;
            set => PlayerPrefs.SetInt(StringConst.LocalKey.AttributeValue.GDPR_VALUE, value ? 1 : 0);
        }

        [IsRemoveAds]
        public static bool IsRemoveAds
        {
            get => PlayerPrefs.GetInt(StringConst.LocalKey.AttributeValue.IS_REMOVE_ADS, 0) != 0;
            set => PlayerPrefs.SetInt(StringConst.LocalKey.AttributeValue.IS_REMOVE_ADS, value ? 1 : 0);
        }

        [DeviceId] public static string DeviceID => SystemInfo.deviceUniqueIdentifier;

        [CurrentLevelMode] public static string CurrentLevelMode => "normal";

        [Country] public static string Country => "global";

        [CurrentLevel] public static int CurrentLevel => 1;
    }
}

