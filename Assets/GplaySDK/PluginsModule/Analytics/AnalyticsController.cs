// Filename: AnalyticsController.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:58 AM 12/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Analytics;

namespace GplaySDK.Analytics
{
    public static class AnalyticsController
    {
        public static event Action<Dictionary<string, Parameter>, HashSet<string>, Dictionary<string, object>>
            OnLevelStartInitialize;

        public static event Action<Dictionary<string, Parameter>, HashSet<string>, Dictionary<string, object>>
            OnLevelEndInitialize;


        public static void LogLevelStart()
        {
            var parameters = new Dictionary<string, Parameter>();
            var eventNames = new HashSet<string>();
            OnLevelStartInitialize?.Invoke(parameters, eventNames, null);
            var parameterArray = parameters.Values.ToArray();
            foreach (var eventName in eventNames)
            {
                FirebaseAnalytics.LogEvent(eventName, parameterArray);
            }
        }

        public static void LogLevelEnd(bool isSuccess)
        {
            var parameters = new Dictionary<string, Parameter>();
            var eventNames = new HashSet<string>();
            var inputData = new Dictionary<string, object>
            {
                { "isSuccess", isSuccess }
            };
            OnLevelEndInitialize?.Invoke(parameters, eventNames, inputData);
            var parameterArray = parameters.Values.ToArray();
            foreach (var eventName in eventNames)
            {
                FirebaseAnalytics.LogEvent(eventName, parameterArray);
            }
        }
    }
}