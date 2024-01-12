// Filename: AdsDatabase.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:43 PM 16/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Const;
using GplaySDK.Core.BaseLib.Utils;
using UnityEngine;

namespace GplaySDK.BaseLib
{
    public static class AdsDatabase
    {
        public static int NumberAdsToday
        {
            get => LocalStorageUtils.Custom.GetInt(StringConst.LocalKey.Ads.Number_Ads_Today, 0);
            set
            {
                LocalStorageUtils.Custom.SetInt(StringConst.LocalKey.Ads.Number_Ads_Today, value);
                LocalStorageUtils.Custom.Save();
            }
        }

        public static int NumberAdsThisSession
        {
            get => LocalStorageUtils.Custom.GetInt(StringConst.LocalKey.Ads.Number_Ads_This_Session, 0);
            set
            {
                LocalStorageUtils.Custom.SetInt(StringConst.LocalKey.Ads.Number_Ads_This_Session, value);
                LocalStorageUtils.Custom.Save();
            }
        }

        [RuntimeInitialize(-11,RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            NumberAdsThisSession = 0;
        }
    }
}