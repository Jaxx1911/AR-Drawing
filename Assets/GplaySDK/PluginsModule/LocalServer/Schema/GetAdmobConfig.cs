// Filename: GetAdmobConfig.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 10:57 16/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.LocalServer.Schema
{
    [Serializable]
    internal class GetAdmobConfigRequest: BaseSchemaRequest
    {
        public string aliasId;
    }
    
    [Serializable]
    public class GetAdmobConfigResponse: BaseSchemaResponse
    {
        public string storeType;
        public string admobAppId;
        public string admobBannerId;
        public string admobInterstitialId;
        public string admobRewardedId;
        public string admobNativeId;
        public string admobOpenAppId;

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