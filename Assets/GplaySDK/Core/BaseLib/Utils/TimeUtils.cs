// Filename: TimeUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 10:29 AM 23/11/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Const;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.Utils
{
    public static class TimeUtils
    {

        public static DateTime FirstOpenTime =>
            LocalStorageUtils.Custom.GetDateTime(StringConst.LocalKey.Time.First_Open_Time, DateTime.Now);


        [RuntimeInitialize(-10050, RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void InitFirstOpenTime()
        {
            if (!SdkDatabase.Common.IsFirstOpen)
            {
                return;
            }

            LocalStorageUtils.Custom.SetDateTime(StringConst.LocalKey.Time.First_Open_Time, DateTime.Now);
            LocalStorageUtils.Custom.Save();
        }
    }
}