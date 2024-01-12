// Filename: JsonUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 10:10 AM 25/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.Utils
{
    public static class JsonUtils
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        public static string ToJson(this object body, bool isPretty = false)
        {
            return JsonConvert.SerializeObject(body, isPretty ? Formatting.Indented : Formatting.None,
                JsonSerializerSettings);
        }

        public static Task<string> ToJsonAsync(this object body, bool isPretty = false)
        {
            return Task.Run(() => ToJson(body, isPretty));
        }

        public static TResult FromJson<TResult>(this string json)
        {
            return JsonConvert.DeserializeObject<TResult>(json, JsonSerializerSettings);
        }

        public static async Task<TResult> FromJsonAsync<TResult>(this string json)
        {
            if (string.IsNullOrEmpty(json)) return default;
            return await Task.Run(() => FromJson<TResult>(json));
        }

        public static IEnumerator DeserializeFakeAsync<TResult>(string path, Action<float> onProgress,
            Action<TResult> onComplete)
        {
            const float minTime = 0.5f;

            string _safePath = path;

            float timeFromStart = 0;

            var handleTask = Task.Run(MainHandle);

            while (timeFromStart < minTime && !handleTask.IsCompleted)
            {
                onProgress?.Invoke(handleTask.IsCompleted
                    ? Mathf.Clamp(timeFromStart / minTime, 0f, 1f)
                    : Mathf.Clamp(timeFromStart / minTime, 0f, 0.95f));
                timeFromStart += Time.deltaTime;
                yield return null;
            }

            onProgress?.Invoke(1f);
            onComplete?.Invoke(handleTask.Result);
            yield break;

            async Task<TResult> MainHandle()
            {
#if UNITY_2021_1_OR_NEWER
                var jsonText = await File.ReadAllTextAsync(_safePath);
#else
                var jsonText = File.ReadAllText(_safePath);
#endif
                return JsonConvert.DeserializeObject<TResult>(jsonText);
            }
        }
    }
}