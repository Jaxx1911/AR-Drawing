// Filename: RemoteConfigController.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:06 PM 02/11/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Firebase.RemoteConfig;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Utils;
using UnityEngine;
using FRemote = Firebase.RemoteConfig.FirebaseRemoteConfig;

namespace GplaySDK.RemoteConfig
{
    public static class RemoteConfigController
    {
        public static class Firebase
        {
            #region Variables

            private static bool _isInit;


            /// <summary>
            /// Only for internal use only.
            /// </summary>
            private static event Action<bool> OnFetchCompleted;

            private static readonly HashSet<string> FirebaseRemoteKeys = new HashSet<string>();

            #endregion

            #region Public Methods

            public static int GetInt(string key, int defaultValue = default)
            {
                if (PreCheck(key)) return defaultValue;

                return FRemote.DefaultInstance.GetValue(key).LongValue > int.MaxValue
                    ? default
                    : (int) FRemote.DefaultInstance.GetValue(key).LongValue;
            }

            public static long GetLong(string key, long defaultValue = default)
            {
                if (PreCheck(key)) return defaultValue;

                return FRemote.DefaultInstance.GetValue(key).LongValue;
            }

            public static float GetFloat(string key, float defaultValue = default)
            {
                if (PreCheck(key)) return defaultValue;

                return (float) FRemote.DefaultInstance.GetValue(key).DoubleValue;
            }

            public static double GetDouble(string key, double defaultValue = default)
            {
                if (PreCheck(key)) return defaultValue;

                return FRemote.DefaultInstance.GetValue(key).DoubleValue;
            }

            public static bool GetBool(string key, bool defaultValue = default)
            {
                if (PreCheck(key)) return defaultValue;

                return FRemote.DefaultInstance.GetValue(key).BooleanValue;
            }

            public static string GetString(string key, string defaultValue = default)
            {
                if (PreCheck(key)) return defaultValue;

                return FRemote.DefaultInstance.GetValue(key).StringValue;
            }

            public static TResult GetEnumByString<TResult>(string key, TResult defaultValue = default)
                where TResult : struct, Enum
            {
                var rawValue = GetString(key);
                return Enum.TryParse(rawValue, true, out TResult result) ? result : defaultValue;
            }

            public static TResult GetEnumByInt<TResult>(string key, TResult defaultValue = default)
                where TResult : struct, Enum
            {
                var rawValue = GetLong(key);
                return Enum.TryParse(rawValue.ToString(), true, out TResult result) ? result : defaultValue;
            }

            public static TResult GetJson<TResult>(string key, TResult defaultValue = default)
            {
                var rawValue = GetString(key);
                return rawValue.FromJson<TResult>() ?? defaultValue;
            }

            public static DateTime GetDateTime(string key, DateTime defaultValue = default)
            {
                var rawValue = GetString(key);
                return DateTime.TryParse(rawValue, out DateTime result) ? result : defaultValue;
            }

            public static void EnsureInitialized([NotNull] Action action)
            {
                if (_isInit)
                {
                    action.Invoke();
                    return;
                }


                OnFetchCompleted += OnFetchSuccess;
                return;

                void OnFetchSuccess(bool status)
                {
                    if (status) action();
                }
            }

            #endregion

            #region Private Methods

            private static async void FetchData()
            {
#if LOG_VERBOSE
                "Firebase Remote Config... Start Fetching".Log();
#endif
                //Invoke first Fetch
#if G_DEBUG
                Task fetchTask =
                    FRemote.DefaultInstance.FetchAsync(TimeSpan.FromSeconds(30));
                var startTime = DateTime.Now;
#else
                Task fetchTask =
                    FRemote.DefaultInstance.FetchAsync(TimeSpan.FromHours(6));
#endif
                await fetchTask;
                await FirstFetchCompleted();


                return;

                async Task FirstFetchCompleted()
                {
                    //Once first fetch is completed, activate the fetched data
                    if (!fetchTask.IsCompleted)
                    {
#if LOG_VERBOSE
                        "Fetch encountered an error".Log();
#endif
                        OnFetchCompleted?.Invoke(false);
                        return;
                    }

                    var info = FRemote.DefaultInstance.Info;
                    switch (info.LastFetchStatus)
                    {
                        case LastFetchStatus.Success:
                            await FRemote.DefaultInstance.FetchAndActivateAsync();
                            FirebaseRemoteKeys.Clear();
                            FirebaseRemoteKeys.UnionWith(FRemote.DefaultInstance.Keys);
#if G_DEBUG && LOG_VERBOSE
                            $"Fetch Firebase remote config success\n{FRemote.DefaultInstance.Info.FetchTime}\nTotal Time: {DateTime.Now - startTime}"
                                .Log(ColorDefine.Emerald);
#endif
                            _isInit = true;
                            OnFetchCompleted?.Invoke(true);
                            return;
                        case LastFetchStatus.Pending:
#if LOG_VERBOSE
                            "Fetch is still on pending".Log();
#endif
                            break;
                        case LastFetchStatus.Failure:
                        default:
#if LOG_ERROR
                            switch (info.LastFetchFailureReason)
                            {
                                case FetchFailureReason.Error:
                                    "Fetch failed for unknown reason".LogError();
                                    break;
                                case FetchFailureReason.Throttled:
                                    $"Fetch throttled until {info.ThrottledEndTime}".LogError();
                                    break;
                                case FetchFailureReason.Invalid:
                                    "Fetch failed due to invalid request".LogError();
                                    break;
                            }
#endif
                            break;
                    }

                    OnFetchCompleted?.Invoke(false);
                }
            }

            /// <summary>
            /// Pre-check before get value. Check if Firebase Remote Config is initialized and contains key.
            /// </summary>
            /// <param name="key"></param>
            /// <returns>Is Error</returns>
            private static bool PreCheck(string key)
            {
                if (!_isInit)
                {
#if LOG_VERBOSE
                    "Firebase Remote Config is not initialized".Log();
#endif
                    return true;
                }

                if (!FirebaseRemoteKeys.Contains(key))
                {
#if LOG_VERBOSE
                    $"Firebase Remote Config doesn't contain key: {key}".Log();
#endif
                    return true;
                }

                return false;
            }

            [RuntimeInitialize(-10200, RuntimeInitializeLoadType.AfterAssembliesLoaded)]
            private static void InitFirebaseRemoteConfig()
            {
                int retryCount = 0;
                OnFetchCompleted += status =>
                {
                    if (status) return;
                    var retryTime = Mathf.Pow(2, Mathf.Min(6, retryCount++));
#if LOG_VERBOSE
                    $"Firebase Remote Config Fetch Failed. Retry in {retryTime} seconds".Log();
#endif
                    CoroutineUtils.StartDelayAction(FetchData, retryTime);
                };
                FetchData();
            }

            #endregion
        }
    }
}