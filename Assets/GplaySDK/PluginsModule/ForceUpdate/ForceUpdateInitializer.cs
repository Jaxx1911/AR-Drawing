// Filename: ForceUpdateInitializer.cs
// Purpose: Run on start of the game to force user to update the game if needed.
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:13 PM 06/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using GplaySDK.Core;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Const;
using GplaySDK.RemoteConfig;
using UnityEngine;

namespace GplaySDK.ForceUpdate
{
    internal static class ForceUpdateInitializer
    {
        private static int _majorVersion;

        [RuntimeInitialize(10, RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void FetchMajorVersion()
        {
            RemoteConfigController.Firebase.EnsureInitialized(() =>
            {
                _majorVersion = RemoteConfigController.Firebase.GetInt(StringConst.RemoteKey.ForceUpdate.Remote_Key, 0);
                OnFetchSuccess();
            });
        }

        private static void OnFetchSuccess()
        {
            if (_majorVersion == 0) return;

            var config = GplaySDKConfig.GetConfig();
            var version = config.versionCode;

            if (_majorVersion <= version) return;

            config.ForceUpdatePrefab.Show(Application.Quit, _majorVersion);
        }
    }
}