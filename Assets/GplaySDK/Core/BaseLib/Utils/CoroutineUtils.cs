// Filename: CoroutineUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:08 AM 04/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.Utils
{
    /// <summary>
    /// Only use as a wrapper for CoroutineRunner. Do not use this class directly.
    /// </summary>
    public static class CoroutineUtils
    {
        public static IEnumerator DelayAction(Action action, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            action?.Invoke();
        }

        /// <summary>
        /// Create new Coroutine to run action after interval time.
        /// Run action immediately after start.
        /// </summary>
        /// <param name="action">Action run each interval</param>
        /// <param name="intervalTime">Interval time in ms</param>
        /// <returns></returns>
        public static IEnumerator IntervalAction(Action action, float intervalTime)
        {
            while (true)
            {
                action?.Invoke();
                yield return new WaitForSeconds(intervalTime / 1000f);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public static uint StartIntervalAction(Action action, float intervalTime)
        {
            return StartCoroutine(IntervalAction(action, intervalTime));
        }

        public static uint StartDelayAction(Action action, float delayTime)
        {
            return StartCoroutine(DelayAction(action, delayTime));
        }

        public static uint StartCoroutine(IEnumerator coroutine)
        {
            return CoroutineRunner.StartCoroutine(coroutine);
        }

        public static void StopCoroutine(uint coroutineId)
        {
            CoroutineRunner.StopCoroutine(coroutineId);
        }
    }
}