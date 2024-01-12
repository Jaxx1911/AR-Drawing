// Filename: RandomUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 1:56 PM 02/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Text;

namespace GplaySDK.Core.BaseLib.Utils
{
    public static class RandomUtils
    {
        public static string GenerateRandomString(int length, string chars = null)
        {
            chars ??= "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[UnityEngine.Random.Range(0, chars.Length)]);
            }

            return result.ToString();
        }
        // ReSharper disable once InconsistentNaming
        public static string GenerateRandomUUID()
        {
            return GenerateRandomString(32,"0123456789abcdef");
        }
    }
}