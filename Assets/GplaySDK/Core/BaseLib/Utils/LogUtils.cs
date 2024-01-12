// Filename: LogUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:58 PM 09/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.Utils
{
    public static class LogUtils
    {
        public static string Color(this string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>";
        }


        private static void BaseLogAction(Action mainAction)
        {
            //Log to custom server...
            mainAction = (() => { }) + mainAction;
            //Reg to main thread
            ThreadUtils.RunOnMainThread(mainAction);
        }


        public static void Log(this string text)
        {
            BaseLogAction(() => Debug.Log(text));
        }

        public static void Log(this string text, Color color)
        {
            BaseLogAction(() => Debug.Log(text.Color(color)));
        }

        public static void Log(this object obj)
        {
            BaseLogAction(() => Debug.Log(obj.GetType().FullName + "\n" + obj.ToJson(true)));
        }

        public static void Log(this object obj, Color color)
        {
            BaseLogAction(() => Debug.Log(obj.GetType().FullName.Color(color) + "\n" + obj.ToJson(true)));
        }

        public static void Log(this string text, object obj)
        {
            BaseLogAction(() => Debug.Log(text + "\n" + obj.ToJson(true)));
        }

        public static void Log(this string text, object obj, Color color)
        {
            BaseLogAction(() => Debug.Log(text.Color(color) + "\n" + obj.ToJson(true)));
        }

        public static void LogError(this string text)
        {
            BaseLogAction(() => Debug.LogError(text));
        }

        public static void LogError(this string text, Color color)
        {
            BaseLogAction(() => Debug.LogError(text.Color(color)));
        }

        public static void LogError(this object obj)
        {
            BaseLogAction(() => Debug.LogError(obj.GetType().FullName + "\n" + obj.ToJson(true)));
        }

        public static void LogError(this object obj, Color color)
        {
            BaseLogAction(() =>
                Debug.LogError(obj.GetType().FullName.Color(color) + "\n" + obj.ToJson(true)));
        }

        public static void LogError(this string text, object obj)
        {
            BaseLogAction(() => Debug.LogError(text + "\n" + obj.ToJson(true)));
        }

        public static void LogError(this string text, object obj, Color color)
        {
            BaseLogAction(() => Debug.LogError(text.Color(color) + "\n" + obj.ToJson(true)));
        }
    }
}