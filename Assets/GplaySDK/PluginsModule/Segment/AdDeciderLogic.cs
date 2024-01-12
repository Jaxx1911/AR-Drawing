// Filename: IAdDecider.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 17:42 28/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.Segment
{
    public abstract class AdDeciderLogic<TPlacement, TSegment>
        where TPlacement : Enum
        where TSegment : AdSegment
    {
        public abstract int Order { get; }
        public abstract bool IsRequirement { get; }
        public virtual uint WeightFail => 0;
        public virtual uint WeightSuccess => 0;

        public abstract TPlacement placementType { get; }

        internal Type segmentType = typeof(TSegment);
        
        protected virtual void Initialize()
        {
        }
        public abstract bool IsShowAds();
        public abstract void FetchConfig();
    }
}