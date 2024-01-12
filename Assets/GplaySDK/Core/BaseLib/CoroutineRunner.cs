// Filename: CoroutineRunner.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:44 AM 04/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;


namespace GplaySDK.Core.BaseLib
{
    internal static class CoroutineRunner
    {
        private static Runner _instance;

        private static bool _isInitialized = false;

        private static readonly Dictionary<uint, Coroutine> ActiveCoroutines = new Dictionary<uint, Coroutine>();

        private static uint _coroutineId = 1;

        public static uint StartCoroutine(IEnumerator coroutine)
        {
            var coroutineId = _coroutineId++;

            if (!ThreadUtils.IsOnMainThread)
            {
#if LOG_VERBOSE
                $"CoroutineRunner is not on main thread... Run later\nId: {coroutineId}".Log();
#endif
                ThreadUtils.RunOnMainThread(MainRunner);
                return coroutineId;
            }
            
#if G_LOG_LOW_LEVEL
            if (!_isInitialized)
            {
#if LOG_VERBOSE
                $"CoroutineRunner is not initialized... Run later\nId: {coroutineId}".Log();
#endif
                ThreadUtils.RunOnMainThread(MainRunner);
                return coroutineId;
            }
#endif

            MainRunner();
            return coroutineId;

            IEnumerator CallbackCoroutine()
            {
                yield return _instance.StartCoroutine(coroutine);
                ActiveCoroutines.Remove(coroutineId);
            }

            void MainRunner()
            {
                var runningCoroutine = _instance.StartCoroutine(CallbackCoroutine());
                //Save to list to stop later
#if LOG_VERBOSE
                $"Start Coroutine: {coroutineId}".Log();
#endif
                ActiveCoroutines.Add(coroutineId, runningCoroutine);
            }
        }


        public static void StopCoroutine(uint coroutineId)
        {
            if (coroutineId == 0)
            {
#if LOG_ERROR
                $"Coroutine is null\n{new System.Diagnostics.StackTrace()}".LogError();
#endif
                return;
            }

            _instance.StopCoroutineById(coroutineId);
        }

        [RuntimeInitialize(-10000, RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            var runner = new GameObject("Coroutine Runner");
            Object.DontDestroyOnLoad(runner);
            _instance = runner.AddComponent<Runner>();
        }

        // Only use to start coroutine
        private class Runner : MonoBehaviour
        {
#if G_LOG_LOW_LEVEL
            // ReSharper disable once InconsistentNaming
            [ShowInInspector]
            private Dictionary<uint,Coroutine> _activeCoroutines => ActiveCoroutines;
#endif
            private void Awake()
            {
                AppStateEventNotifier.AppStateChanged += UnityBridge.OnApplicationStateChanged;
#if LOG_VERBOSE
                Debug.Log("Coroutine Runner Awake");
#endif
                _isInitialized = true;
            }

            // ReSharper disable once MemberHidesStaticFromOuterClass
            private void OnApplicationPause(bool pauseStatus)
            {
                UnityBridge.OnApplicationPause?.Invoke(pauseStatus);
            }

            // ReSharper disable once MemberHidesStaticFromOuterClass
            private void OnApplicationFocus(bool hasFocus)
            {
                UnityBridge.OnApplicationFocus?.Invoke(hasFocus);
            }

            // Use this for stop internal coroutine.
            [Button]
            public void StopCoroutineById(uint id)
            {
                if (!ActiveCoroutines.TryGetValue(id, out Coroutine coroutine)) return;
                StopCoroutine(coroutine);
                ActiveCoroutines.Remove(id);
            }
        }
    }
}