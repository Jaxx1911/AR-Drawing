// Filename: CurrentLevelAttribute.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:43 AM 29/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

namespace GplaySDK.Core.BaseLib.Attribute
{
    public class CurrentLevelAttribute: RequirePropertyAttribute
    {
        private const string RequirePropertyKey = "CurrentLevel";
        
        public static int Value
        {
            get => GetIntValue(RequirePropertyKey);
            set => SetIntValue(RequirePropertyKey, value);
        }
        private static CurrentLevelAttribute _instance;
        public CurrentLevelAttribute() : base(RequirePropertyKey, ValueType.Int)
        {
            _instance = this;
        }
    }
}