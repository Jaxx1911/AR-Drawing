// Filename: AdDecider.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 17:28 28/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;

namespace GplaySDK.Segment
{
    internal class AdDecider<TPlacement, TSegment>
        where TPlacement : Enum
        where TSegment : AdSegment
    {
        private readonly List<AdDeciderLogic<TPlacement, TSegment>> _logicList =
            new List<AdDeciderLogic<TPlacement, TSegment>>();


        internal void AddLogic(AdDeciderLogic<TPlacement, TSegment> logic)
        {
            _logicList.Add(logic);
        }

        internal void FetchConfig()
        {
            foreach (var logic in _logicList)
            {
                logic.FetchConfig();
            }
        }

        internal bool IsShowAds()
        {
            var weight = 0L;
            var isRequirement = true;
            foreach (var logic in _logicList)
            {
                if (logic.IsShowAds())
                {
                    weight += logic.WeightSuccess;
                    continue;
                }

                if (logic.IsRequirement)
                {
                    isRequirement = false;
                    break;
                }

                weight -= logic.WeightFail;
            }

            if (!isRequirement) return false;
            return weight > 0;
        }
    }
}