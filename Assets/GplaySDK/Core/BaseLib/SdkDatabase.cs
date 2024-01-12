// Filename: SdkDatabase.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 6:18 PM 16/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Const;
using GplaySDK.Core.BaseLib.Utils;
using UnityEngine;

namespace GplaySDK.Core.BaseLib
{
    public static class SdkDatabase
    {
        public static class Common
        {
            private static bool _isFirstOpenThisSession = false;

            public static bool IsFirstOpen
            {
                get => _isFirstOpenThisSession ||
                       !LocalStorageUtils.Custom.HasKey(StringConst.LocalKey.Common.Is_First_Open);
                private set => LocalStorageUtils.Custom.SetBool(StringConst.LocalKey.Common.Is_First_Open, value);
            }


            [RuntimeInitialize(-11000, RuntimeInitializeLoadType.AfterAssembliesLoaded)]
            private static void InitCommonDatabase()
            {
                if (!IsFirstOpen) return;
                _isFirstOpenThisSession = true;
                IsFirstOpen = true;
            }
        }
        
    }
}