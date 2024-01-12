// Filename: For2021.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:43 PM 13/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GplaySDK.Core.BaseLib.OldWrapper
{
#if !UNITY_2021_1_OR_NEWER
    public static class For2021
    {
        public static void TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (dic.ContainsKey(key))
            {
                return;
            }

            dic.Add(key, value);
        }
    }
#endif
}