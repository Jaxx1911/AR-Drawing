// Filename: SegmentController.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:8 11/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;

namespace GplaySDK.Segment
{
    public static class SegmentController
    {
        private static readonly Dictionary<Type, object> SegmentDict = new Dictionary<Type, object>();

        public static TResult GetSegment<TResult>() where TResult : BaseSegment
        {
            var type = typeof(TResult);
            if (!SegmentDict.ContainsKey(type))
            {
                SegmentDict[type] = Activator.CreateInstance(type);
            }

            return (TResult) SegmentDict[type];
        }
    }
}