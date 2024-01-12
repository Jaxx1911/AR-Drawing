// Filename: DeviceIdAttribute.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:01 PM 04/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

namespace GplaySDK.Core.BaseLib.Attribute
{
    public class DeviceIdAttribute : RequirePropertyAttribute
    {
        private const string RequirePropertyKey = "DeviceId";
        public static string Value
        {
            get => GetStringValue(RequirePropertyKey);
            set => SetStringValue(RequirePropertyKey, value);
        }

        private static DeviceIdAttribute _instance;

        public DeviceIdAttribute() : base(RequirePropertyKey, ValueType.String)
        {
            _instance = this;
        }
    }
}