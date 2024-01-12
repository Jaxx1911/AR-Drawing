// Filename: BaseSegment.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:14 11/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.Segment
{
    [Serializable]
    public abstract class BaseSegment
    {
        protected bool isEnable;
        
        protected abstract void FetchConfig();

    }
}