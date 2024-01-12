// Filename: CountryAttribute.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:03 PM 04/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.Core.BaseLib.Attribute
{
    public class CountryAttribute : RequirePropertyAttribute
    {
        private const string RequirePropertyKey = "Country";

        public static string Value
        {
            get => GetStringValue(RequirePropertyKey);
            set => SetStringValue(RequirePropertyKey, value);
        }

        public static CountryCode CountryCode
        {
            get
            {
                if (Enum.TryParse(Value, true, out CountryCode code))
                    return code;
                return CountryCode.GLOBAL;
            }
        }


        public CountryAttribute() : base(RequirePropertyKey, ValueType.String)
        {
        }
    }
}