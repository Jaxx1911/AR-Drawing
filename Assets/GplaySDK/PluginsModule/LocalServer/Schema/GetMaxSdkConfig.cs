// Filename: GetMaxSdkConfig.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 8:56 16/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.LocalServer.Schema
{
    [Serializable]
    public class GetMaxSdkConfigRequest : BaseSchemaRequest
    {
        public string aliasId;
    }

    [Serializable]
    public class GetMaxSdkConfigResponse : BaseSchemaResponse
    {
        public string storeType;
        public string maxSdkKey;
        public string maxSdkBannerId;
        public string maxSdkInterstitialId;
        public string maxSdkRewardedId;
        public string maxSdkNativeId;

        public bool IsAndroid()
        {
            return base.IsAndroid(storeType);
        }

        public bool IsIos()
        {
            return base.IsIos(storeType);
        }
    }
}