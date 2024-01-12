// Filename: UnityBridge.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 3:06 PM 02/11/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using GoogleMobileAds.Common;

namespace GplaySDK.Core.BaseLib
{
    public static class UnityBridge
    {
                
        public static Action<bool> OnApplicationPause;
        
        public static Action<bool> OnApplicationFocus;
        
        public static Action<AppState> OnApplicationStateChanged;
    }
}