// Filename: SdkConfigBase.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 14:20 15/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.LocalServer.Schema
{
    
    [Serializable]
    public class BaseSchemaRequest
    {
        
    }

    [Serializable]
    public class BaseSchemaResponse
    {
        public string message;
        public bool isSuccess;
        
        protected bool IsAndroid(string storeType)
        {
            if (string.IsNullOrEmpty(storeType)) return false;
            storeType = storeType.ToLower();
            // Do not merge condition need to support lower version
            // ReSharper disable once MergeIntoLogicalPattern
            return storeType == "google_play" || storeType == "android";
        }
        
        protected bool IsIos(string storeType)
        {
            if (string.IsNullOrEmpty(storeType)) return false;
            storeType = storeType.ToLower();
            // Do not merge condition need to support lower version
            // ReSharper disable once MergeIntoLogicalPattern
            return storeType == "google_play" || storeType == "android";
        }
    }
}