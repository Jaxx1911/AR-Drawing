// Filename: IsRemoveAdsAttribute.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:50 PM 09/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using GplaySDK.Core.BaseLib.Attribute;

namespace GplaySDK.BaseLib.RequireProperty
{
    public class IsRemoveAdsAttribute : RequirePropertyAttribute
    {
        private const string RequirePropertyKey = "IsRemoveAds";

        public static bool Value
        {
            get => GetBoolValue(RequirePropertyKey);
            set => SetBoolValue(RequirePropertyKey, value);
        }

        public IsRemoveAdsAttribute() : base(RequirePropertyKey, ValueType.Bool)
        {
        }
    }
}