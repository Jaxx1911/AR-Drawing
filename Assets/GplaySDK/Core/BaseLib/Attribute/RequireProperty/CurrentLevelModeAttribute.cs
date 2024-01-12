// Filename: CurrentLevelAttribute.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:43 AM 29/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.Core.BaseLib.Attribute
{
    public class CurrentLevelModeAttribute : RequirePropertyAttribute
    {
        private const string RequirePropertyKey = "CurrentLevelMode";
        public static string Value
        {
            get => GetStringValue(RequirePropertyKey);
            set => SetStringValue(RequirePropertyKey, value);
        }

        private static CurrentLevelModeAttribute _instance;

        public CurrentLevelModeAttribute() : base(RequirePropertyKey, ValueType.String)
        {
            _instance = this;
        }
    }
}