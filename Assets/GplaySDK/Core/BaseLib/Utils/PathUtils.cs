// Filename: PathUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:15 PM 02/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.IO;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.Utils
{
    public static class PathUtils
    {
        public static string GetStreamingAssetFullPath(string path)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return Path.Combine(Application.streamingAssetsPath, path);
            }
#endif
            return Path.Combine(LocalStorageUtils.StreamingAssetsPath ?? Application.streamingAssetsPath, path);
        }

        public static string GetPersistentDataFullPath(string path)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return Path.Combine(Application.persistentDataPath, path);
            }
#endif
            return Path.Combine(LocalStorageUtils.PersistentDataPath ?? Application.persistentDataPath, path);
        }
#if UNITY_EDITOR
        [UnityEditor.MenuItem("LA/Open Persistent Folder")]
        private static void OpenPersistentFolder()
        {
            var path = $"\"{Application.persistentDataPath}\"";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
            else if (Application.platform == RuntimePlatform.LinuxEditor)
            {
                System.Diagnostics.Process.Start("xdg-open", path);
                Debug.Log("Linux");
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                System.Diagnostics.Process.Start("open", path);
            }
        }

        [UnityEditor.MenuItem("LA/Clear Persistent Folder")]
        private static void ClearPersistentFolder()
        {
            var path = Application.persistentDataPath;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
#endif
    }
}