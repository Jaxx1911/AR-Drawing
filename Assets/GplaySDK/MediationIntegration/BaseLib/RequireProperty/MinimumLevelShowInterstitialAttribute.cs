// Filename: MinimumLevelShowAdsAttribute.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:09 PM 16/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using GplaySDK.Core.BaseLib.Attribute;
using JetBrains.Annotations;

namespace GplaySDK.BaseLib.RequireProperty
{
    [MeansImplicitUse]
    public class MinimumLevelShowInterstitialAttribute : RequirePropertyAttribute
    {
        private const string RequirePropertyKey = "MinimumLevelShowInterstitial";

        public static int Value
        {
#if G_SKIP_REQUIRE_PROPERTY
            get => -1;
#else
            get => GetIntValue(RequirePropertyKey);
#endif
            set => SetIntValue(RequirePropertyKey, value);
        }

        public MinimumLevelShowInterstitialAttribute() : base(RequirePropertyKey, ValueType.Int)
        {
        }
    }
}