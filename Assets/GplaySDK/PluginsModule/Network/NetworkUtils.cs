// Filename: NetworkUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:2 14/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections;
using System.Threading.Tasks;

namespace GplaySDK.Network
{
    public static class NetworkUtils
    {
        public static void Post(string url, string body, Action<string> onCompleted)
        {
            NetworkController.Post(url, body).ContinueWith(task =>
            {
                onCompleted?.Invoke(task.IsFaulted ? null : task.Result);
            });
        }
        public static Task<string> PostAsync(string url, string body)
        {
            return NetworkController.Post(url, body);
        }
        public static string Post(string url, string body)
        {
            return NetworkController.Post(url, body).Result;
        }
    }
}