// Filename: LocalStorageUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:51 PM 16/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using GplaySDK.Core.BaseLib.Attribute;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.Utils
{
    public static class LocalStorageUtils
    {
        #region Const Data

        public static string PersistentDataPath { get; private set; }

        public static string StreamingAssetsPath { get; private set; }

        [RuntimeInitialize(-11050, RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            PersistentDataPath = Application.persistentDataPath;
            StreamingAssetsPath = Application.streamingAssetsPath;
            Custom.InitCustomType();
        }

        #endregion


        /// <summary>
        /// Use Default PlayerPrefs to save data. Must call in main thread
        /// </summary>
        [Obsolete("Use Custom instead", false)]
        public static class BuildIn
        {
            #region Low Level API

#if G_DEBUG

            public static event Action OnSaveDatabase;


            private static readonly Dictionary<string, Action<object>> OnValueChangedMap =
                new Dictionary<string, Action<object>>();


            /// <summary>
            /// Register a callback when value of key changed.
            /// </summary>
            /// <param name="key">Key of that field</param>
            /// <param name="onChange">Action when that value is change. Res Value can be null which present delete key situation</param>
            /// <typeparam name="TValue">Type of that field</typeparam>
            public static void RegisterOnValueChanged<TValue>(string key, Action<TValue> onChange)
            {
                if (OnValueChangedMap.ContainsKey(key))
                {
                    OnValueChangedMap[key] += res => onChange.Invoke(res is TValue value ? value : default);
                }
                else
                {
                    OnValueChangedMap.Add(key, res => onChange.Invoke(res is TValue value ? value : default));
                }
            }

            private static void OnValueChanged(string key, object value)
            {
                if (OnValueChangedMap.TryGetValue(key, out var action))
                {
                    action?.Invoke(value);
                }
            }
#endif

            #endregion

            #region Utils

            public static void Save()
            {
#if G_DEBUG
                OnSaveDatabase?.Invoke();
#endif
                PlayerPrefs.Save();
            }


            public static bool HasKey(string key)
            {
                return PlayerPrefs.HasKey(key);
            }

            public static void DeleteKey(string key)
            {
#if G_DEBUG
                OnValueChanged(key, null);
#endif
                PlayerPrefs.DeleteKey(key);
            }

            #endregion

            #region Set Value

            public static void SetInt(string key, int value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                PlayerPrefs.SetInt(key, value);
            }

            public static void SetFloat(string key, float value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                PlayerPrefs.SetFloat(key, value);
            }

            public static void SetString(string key, string value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                PlayerPrefs.SetString(key, value);
            }

            public static void SetBool(string key, bool value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                PlayerPrefs.SetInt(key, value ? 1 : 0);
            }

            [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
            public static void SetEnum<T>(string key, T value) where T : Enum
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                if (value is null)
                {
                    // Delete key
                    DeleteKey(key);
                    return;
                }

                // ReSharper disable once PatternNeverMatches
                if (value is int parseInt)
                {
                    PlayerPrefs.SetInt(key, parseInt);
                }
#if LOG_ERROR
                $"SetEnum not support type {typeof(T)}".LogError();
#endif
            }

            public static void SetDateTime(string key, DateTime value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                PlayerPrefs.SetString(key, value.Ticks.ToString(CultureInfo.InvariantCulture));
            }

            #endregion

            #region Get Value

            public static int GetInt(string key, int defaultValue = 0)
            {
                return PlayerPrefs.GetInt(key, defaultValue);
            }

            public static float GetFloat(string key, float defaultValue = 0)
            {
                return PlayerPrefs.GetFloat(key, defaultValue);
            }

            public static string GetString(string key, string defaultValue = "")
            {
                return PlayerPrefs.GetString(key, defaultValue);
            }

            public static bool GetBool(string key, bool defaultValue = false)
            {
                return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
            }

            [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
            public static T GetEnum<T>(string key, T defaultValue = default) where T : Enum
            {
                // ReSharper disable once PatternNeverMatches
                if (defaultValue is int parseInt)
                {
                    return (T) Enum.ToObject(typeof(T), PlayerPrefs.GetInt(key, parseInt));
                }
#if LOG_ERROR
                $"GetEnum not support type {typeof(T)}".LogError();
#endif
                return defaultValue;
            }

            public static DateTime GetDateTime(string key, DateTime defaultValue = default)
            {
                const string defaultString = "Get Failed";
                var ticks = GetString(key, defaultString);
                if (long.TryParse(ticks, out long result))
                {
                    return new DateTime(result);
                }
#if LOG_ERROR
                $"GetDateTime failed\nIs has key: {HasKey(key)}\nKey Value: {ticks}".LogError();
#endif
                return defaultValue;
            }
            #endregion
        }

        /// <summary>
        /// Use BinaryFormatter to save data. Save in PersistentDataPath. Can call in any thread.
        /// </summary>
        public static class Custom
        {
            private const string SaveName = "GplaySDK_LocalStorage.dat";

            private static string _savePath;

            private static ulong _saveCount = 0;

            private static ulong _lastSaveCount = 0;

            private static Dictionary<string, object> _cache;

            #region Low Level API

#if G_DEBUG
            public static event Action OnSaveDatabase;

            private static readonly Dictionary<string, Action<object>> OnValueChangedMap =
                new Dictionary<string, Action<object>>();

            /// <summary>
            /// Register a callback when value of key changed.
            /// </summary>
            /// <param name="key">Key of that field</param>
            /// <param name="onChange">Action when that value is change. Res Value can be null which present delete key situation</param>
            /// <typeparam name="TValue">Type of that field</typeparam>
            public static void RegisterOnValueChanged<TValue>(string key, Action<TValue> onChange)
            {
                if (OnValueChangedMap.ContainsKey(key))
                {
                    OnValueChangedMap[key] += res => onChange.Invoke(res is TValue value ? value : default);
                }
                else
                {
                    OnValueChangedMap.Add(key, res => onChange.Invoke(res is TValue value ? value : default));
                }
            }

            private static void OnValueChanged(string key, object value)
            {
                if (OnValueChangedMap.TryGetValue(key, out var action))
                {
                    action?.Invoke(value);
                }
            }


#endif

            #endregion

            #region Utils

            internal static void InitCustomType()
            {
                _savePath = PathUtils.GetPersistentDataFullPath(SaveName);
                //Main Init
                Load();
                Task.Run(SaveInterval);
            }

            private static async Task SaveInterval()
            {
                // 5 times slower than normal interval
                int instanceAsyncInterval = BaseLibConfig.Instance.asyncInterval * 5;
                while (true)
                {
                    if (_lastSaveCount != _saveCount)
                    {
                        Save();
                    }

                    await Task.Delay(instanceAsyncInterval);
                }
                // ReSharper disable once FunctionNeverReturns
            }

            private static void Load()
            {
                //Use BinaryFormatter to load data
                if (!File.Exists(_savePath))
                {
                    _cache = new Dictionary<string, object>();
                    return;
                }

                using var stream = new FileStream(_savePath, FileMode.Open);
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                if (_cache == null)
                {
                    _cache = (Dictionary<string, object>) formatter.Deserialize(stream);
                }
                else
                    lock (_cache)
                    {
                        _cache = (Dictionary<string, object>) formatter.Deserialize(stream);
                    }

#if LOG_VERBOSE
                $"Load Local Storage {_cache.ToJson(true)}".Log();
#endif

                stream.Close();
            }

            public static void Save()
            {
#if G_DEBUG
                OnSaveDatabase?.Invoke();
#endif
                //Use BinaryFormatter to save data in Cache to file
                using var stream = new FileStream(_savePath, FileMode.Create);
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                lock (_cache)
                {
                    formatter.Serialize(stream, _cache);
                }

                stream.Close();
                _lastSaveCount = _saveCount;
            }

            public static bool HasKey(string key)
            {
                return _cache.ContainsKey(key);
            }

            public static void DeleteKey(string key)
            {
#if G_DEBUG
                OnValueChanged(key, null);
#endif
                _cache.Remove(key);
                _saveCount++;
            }

            #endregion

            #region Set Value

            public static void SetInt(string key, int value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                _cache[key] = value;
                _saveCount++;
            }

            public static void SetFloat(string key, float value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                _cache[key] = value;
                _saveCount++;
            }

            public static void SetDouble(string key, double value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                _cache[key] = value;
                _saveCount++;
            }

            public static void SetString(string key, string value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                _cache[key] = value;
                _saveCount++;
            }

            public static void SetBool(string key, bool value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                _cache[key] = value;
                _saveCount++;
            }

            public static void SetEnum<T>(string key, T value) where T : Enum
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                _cache[key] = value;
                _saveCount++;
            }

            public static void SetDateTime(string key, DateTime value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                _cache[key] = value;
                _saveCount++;
            }

            public static void SetObject(string key, object value)
            {
#if G_DEBUG
                OnValueChanged(key, value);
#endif
                _cache[key] = value;
                _saveCount++;
            }

            #endregion

            #region Get Value

            public static int GetInt(string key, int defaultValue = 0)
            {
                return _cache.TryGetValue(key, out var res) ? (int) res : defaultValue;
            }

            public static float GetFloat(string key, float defaultValue = 0)
            {
                return _cache.TryGetValue(key, out var res) ? (float) res : defaultValue;
            }

            public static double GetDouble(string key, double defaultValue = 0)
            {
                return _cache.TryGetValue(key, out var res) ? (double) res : defaultValue;
            }

            public static string GetString(string key, string defaultValue = "")
            {
                return _cache.TryGetValue(key, out var res) ? (string) res : defaultValue;
            }

            public static bool GetBool(string key, bool defaultValue = false)
            {
                return _cache.TryGetValue(key, out var res) ? (bool) res : defaultValue;
            }

            public static T GetEnum<T>(string key, T defaultValue = default) where T : Enum
            {
                return _cache.TryGetValue(key, out var res) ? (T) res : defaultValue;
            }

            public static DateTime GetDateTime(string key, DateTime defaultValue = default)
            {
                return _cache.TryGetValue(key, out var res) ? (DateTime) res : defaultValue;
            }

            public static TResult GetObject<TResult>(string key, TResult defaultValue = default)
            {
                return _cache.TryGetValue(key, out var res) ? (TResult) res : defaultValue;
            }

            #endregion
        }
    }
}