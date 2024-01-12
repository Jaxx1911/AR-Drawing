// Filename: GetBasicSdkConfigResponse.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:14 15/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.LocalServer.Schema
{
    [Serializable]
    public class GetBasicSdkConfigRequest : BaseSchemaRequest
    {
        public string aliasId;
    }

    [Serializable]
    public class GetBasicSdkConfigResponse : BaseSchemaResponse
    {
        public string id;
        public string aliasId;
        public string name;
        public string packageName;
        public string storeType;
        public string storeId;
        public string description;

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