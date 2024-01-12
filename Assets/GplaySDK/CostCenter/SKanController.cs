// Filename: SKanController.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:45 AM 23/11/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

//#define DEV

using System;
using GplaySDK.Ads;
using GplaySDK.Core.BaseLib;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Const;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.MediationIntegration.BaseLib;
using GplaySDK.RemoteConfig;
using UnityEngine;

namespace GplaySDK.CostCenter
{
    public static class SKanController
    {
        private static SKanSchema _schema;

        private static DateTime _maxCountingTime;

        private static int _conversionValue = -1;

        private static bool IsUseSKan => DateTime.Now < _maxCountingTime;

        private static double FirstDayRevenue
        {
            get => LocalStorageUtils.Custom.GetDouble(StringConst.LocalKey.SKan.First_Day_Revenue, 0);
            set
            {
                LocalStorageUtils.Custom.SetDouble(StringConst.LocalKey.SKan.First_Day_Revenue, value);
                LocalStorageUtils.Custom.Save();
            }
        }


        [RuntimeInitialize(1, RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
#if !UNITY_IOS && !DEV
            return;
#endif
            _maxCountingTime = TimeUtils.FirstOpenTime.AddHours(24);

            if (!IsUseSKan)
            {
                return;
            }

#if UNITY_IOS || DEV
            if (SdkDatabase.Common.IsFirstOpen)
            {
                Unity.Advertisement.IosSupport.SkAdNetworkBinding.SkAdNetworkRegisterAppForNetworkAttribution();
            }
#endif

            var schemaValueRaw = RemoteConfigController.Firebase.GetString(StringConst.RemoteKey.SKan.Remote_Key);
            if (!string.IsNullOrEmpty(schemaValueRaw)) return;
            _schema.ImportSchemaFromString(schemaValueRaw);
            BaseAdsConfig.OnAdPaid += (_, __, revenue, ___) => OnHaveRevenueInFirstDay(revenue);
        }

        private static void OnHaveRevenueInFirstDay(double revenue)
        {
#if !UNITY_IOS && !DEV
            return;
#endif
#if UNITY_IOS
            if (!IsUseSKan) return;
            FirstDayRevenue += revenue;
            var convValue = _schema.GetConversionValue(FirstDayRevenue);
            if (_conversionValue != convValue)
            {
                Unity.Advertisement.IosSupport.SkAdNetworkBinding.SkAdNetworkUpdateConversionValue(convValue);
                _conversionValue = convValue;
            }
#endif
        }
    }
}