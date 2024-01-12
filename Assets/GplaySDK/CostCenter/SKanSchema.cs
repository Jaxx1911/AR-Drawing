// Filename: SKanSchema.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:39 PM 22/11/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using GplaySDK.Core.BaseLib;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GplaySDK.CostCenter
{
    [Serializable]
    public class SKanSchema
    {
        [SerializeField, HideInInspector] private CountryCode region;
        [SerializeField, HideInInspector] private FloatRange[] schemaValue;

        [ShowInInspector, ReadOnly] public CountryCode Region => region;

        [ShowInInspector, ReadOnly] public FloatRange[] SchemaValue => schemaValue;


        public int GetConversionValue(double revenue)
        {
            if (schemaValue == null || schemaValue.Length == 0)
            {
                return 0;
            }

            for (int i = 0; i < schemaValue.Length; i++)
            {
                if (schemaValue[i].IsInRange((float) revenue))
                {
                    return i;
                }
            }

            return schemaValue.Length;
        }


        [Button]
        private void SetRegion(CountryCode region)
        {
            this.region = region;
        }

        [Button]
        public void ImportSchemaFromString(string schemaValueRaw)
        {
            //Example string: 0,0.00001,0.004847,0.008704,0.013114,0.018235,0.025302,0.03416,0.046187,0.064977,0.095581,0.170253,5.779143
            var schemaValueRawArray = schemaValueRaw.Split(',');
            var schemaValueList = new List<FloatRange>();
            var min = float.Parse(schemaValueRawArray[0]);
            for (int i = 1; i < schemaValueRawArray.Length; i ++)
            {
                var max = float.Parse(schemaValueRawArray[i]);
                schemaValueList.Add(new FloatRange(min, max));
                min = max;
            }

            schemaValue = schemaValueList.ToArray();
        }
    }
}