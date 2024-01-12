// Filename: InterstitialCooldownAttribute.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:46 AM 09/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

namespace GplaySDK.Core.BaseLib.Attribute
{
    public class InterstitialCooldownAttribute : RequirePropertyAttribute
    {
        private const string RequirePropertyKey = "InterstitialCooldown";

        public static int Value
        {
#if G_SKIP_REQUIRE_PROPERTY
            get => 0;
#else
            get => GetIntValue(RequirePropertyKey);
#endif
            set => SetIntValue(RequirePropertyKey, value);
        }

        public InterstitialCooldownAttribute() : base(RequirePropertyKey, ValueType.Int)
        {
        }
    }
}