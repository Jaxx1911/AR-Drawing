// Filename: CostCenter.cs
// Purpose: Cost Center for GplaySDK
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 1:46 PM 22/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using GplaySDK.Analytics;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Utils;
using UnityEngine;
using UnityEngine.Networking;

#if !UNITY_2021_1_OR_NEWER
using GplaySDK.Core.BaseLib.OldWrapper;
#endif

namespace GplaySDK.CostCenter
{
    public static class FirebaseLog
    {
        private const int MaximumRetry = 5;
        
        private const string FirstOpenKey = "CC_first_open";
        private static bool IsFirstOpen
        {
            get
            {
                if (PlayerPrefs.HasKey(FirstOpenKey))
                {
                    int isFirst = PlayerPrefs.GetInt(FirstOpenKey);
                    return isFirst <= 0 && isFirst > -MaximumRetry;
                }

                return true;
            }
            set
            {
                if (!value)
                {
                    PlayerPrefs.SetInt(FirstOpenKey, 1);
                    return;
                }

                if (!PlayerPrefs.HasKey(FirstOpenKey))
                {
                    PlayerPrefs.SetInt(FirstOpenKey, 0);
                    return;
                }

                int lastFirst = PlayerPrefs.GetInt(FirstOpenKey);
                PlayerPrefs.SetInt(FirstOpenKey, lastFirst - 1);
            }
        }
        
        [RuntimeInitialize(-50, RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeLogEventCostCenter()
        {
            AnalyticsController.OnLevelStartInitialize += OnLevelStartInitialize;
            AnalyticsController.OnLevelEndInitialize += OnLevelEndInitialize;
            CoroutineUtils.StartCoroutine(AttributionLog());
        }

        private static void OnLevelStartInitialize(Dictionary<string, Parameter> parameters, HashSet<string> eventNames,
            Dictionary<string, object> _)
        {
            parameters.TryAdd(FirebaseAnalytics.ParameterLevel,
                new Parameter(FirebaseAnalytics.ParameterLevel, CurrentLevelAttribute.Value));
            parameters.TryAdd("level_mode", new Parameter("level_mode", CurrentLevelModeAttribute.Value));
            eventNames.Add(FirebaseAnalytics.EventLevelStart);
        }

        private static void OnLevelEndInitialize(Dictionary<string, Parameter> parameters, HashSet<string> eventNames,
            Dictionary<string, object> inputData)
        {
            parameters.TryAdd(FirebaseAnalytics.ParameterLevel,
                new Parameter(FirebaseAnalytics.ParameterLevel, CurrentLevelAttribute.Value));
            parameters.TryAdd("level_mode", new Parameter("level_mode", CurrentLevelModeAttribute.Value));
            parameters.TryAdd(FirebaseAnalytics.ParameterSuccess, new Parameter(FirebaseAnalytics.ParameterSuccess,
                ((bool) inputData["isSuccess"]).ToString()));
            eventNames.Add(FirebaseAnalytics.EventLevelEnd);
        }

        private static IEnumerator AttributionLog()
        {
            if (!IsFirstOpen) yield break;

            string bundleId = Application.identifier;
            string platform = Application.platform == RuntimePlatform.Android ? "android" : "ios";

            System.Threading.Tasks.Task<string> task =
                FirebaseAnalytics.GetAnalyticsInstanceIdAsync();
            yield return new WaitUntil(() => task.IsCompleted);

            string fbAppInstanceId = task.Result;

            Debug.Log($"{bundleId} - {platform}: {fbAppInstanceId}");
            UnityWebRequest www =
                UnityWebRequest.Get(
                    $"https://attribution.costcenter.net/appopen?bundle_id={bundleId}&platform={platform}&firebase_app_instance_id={fbAppInstanceId}");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
#if LOG_VERBOSE
                Debug.Log("CCAttribution CallAppOpen: success");
#endif
            }
            else
            {
#if LOG_ERROR
                $"CCAttribution CallAppOpen: Failed\n{www.result}".LogError();
#endif
            }
        }
    }
}