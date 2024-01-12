// Filename: FloatRange.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:39 PM 22/11/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.Core.BaseLib
{
    [Serializable]
    public struct FloatRange
    {
        public float min;
        public float max;
        
        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        public bool IsInRange(float value)
        {
            return value >= min && value < max;
        }
    }
}