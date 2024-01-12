// Filename: GDPRValueAttribute.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:16 AM 03/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using GplaySDK.Core.BaseLib.Attribute;
using UnityEngine;

namespace GplaySDK.BaseLib.RequireProperty
{
    public class GdprValueAttribute : RequirePropertyAttribute
    {
        private const string RequirePropertyKey = "GdprValue";
        
        public static bool Value
        {
            get => GetBoolValue(RequirePropertyKey);
            set => SetBoolValue(RequirePropertyKey, value);
        }
        


        public GdprValueAttribute() : base(RequirePropertyKey, ValueType.Bool)
        {
        }
    }
}