// Filename: FileUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 10:42 AM 05/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.Utils
{
    public static class StreamingAssetUtils
    {
        //Temporary disable this function if version is lower than 2021.1.
        //TODO: Write a FileUtils which replace default File class which support async read/write all version
        public static async Task<byte[]> ReadAllBytesAsync(string streamingAssetPath, bool isUseCache)
        {
            var ssPath = PathUtils.GetStreamingAssetFullPath(streamingAssetPath);
            var pPath = PathUtils.GetPersistentDataFullPath(streamingAssetPath);
#if G_LOG_LOW_LEVEL
            Debug.Log("StreamingAssetUtils.ReadAllBytesAsync: " + ssPath + " " + pPath);
#endif
            //Only Android need to use Network Request to read StreamingAsset
#if UNITY_EDITOR || UNITY_IOS
            if (isUseCache)
            {
                if (File.Exists(pPath))
                {
#if UNITY_2021_1_OR_NEWER
                    return await File.ReadAllBytesAsync(pPath);
#else
                    return File.ReadAllBytes(pPath);
#endif
                }
            }
#if UNITY_2021_1_OR_NEWER
            await File.WriteAllBytesAsync(pPath, await File.ReadAllBytesAsync(ssPath));
#else
            File.WriteAllBytes(pPath, File.ReadAllBytes(ssPath));
#endif


#if UNITY_2021_1_OR_NEWER
            return await File.ReadAllBytesAsync(pPath);
#else
            return File.ReadAllBytes(pPath);
#endif
#elif UNITY_ANDROID
            if (isUseCache)
            {
                if (File.Exists(pPath))
                {
#if UNITY_2021_1_OR_NEWER
                    return await File.ReadAllBytesAsync(pPath);
#else
                    return File.ReadAllBytes(pPath);
#endif
                }
            }


            var req = new UnityEngine.Networking.UnityWebRequest(ssPath);
            req.downloadHandler = new UnityEngine.Networking.DownloadHandlerFile(pPath);
            req.SendWebRequest();
            while (!req.isDone)
            {
                // Ignored
            }

            if (req.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + req.error);
                throw new System.Exception(req.error);
            }

#if UNITY_2021_1_OR_NEWER
            return await File.ReadAllBytesAsync(pPath);
#else
            return File.ReadAllBytes(pPath);
#endif
#endif
        }

        public static async Task<string> ReadAllTextAsync(string streamingAssetPath, bool isUseCache = false)
        {
            return Encoding.UTF8.GetString(await ReadAllBytesAsync(streamingAssetPath, isUseCache));
        }


        public static byte[] ReadAllBytes(string streamingAssetPath, bool isUseCache)
        {
            var ssPath = PathUtils.GetStreamingAssetFullPath(streamingAssetPath);
            var pPath = PathUtils.GetPersistentDataFullPath(streamingAssetPath);
#if G_LOG_LOW_LEVEL
            Debug.Log("StreamingAssetUtils.ReadAllBytesAsync: " + ssPath + " " + pPath);
#endif
            //Only Android need to use Network Request to read StreamingAsset
#if UNITY_EDITOR || UNITY_IOS
            if (isUseCache)
            {
                if (File.Exists(pPath))
                {
                    return File.ReadAllBytes(pPath);
                }
            }

            File.WriteAllBytes(pPath, File.ReadAllBytes(ssPath));

            return File.ReadAllBytes(ssPath);
#elif UNITY_ANDROID
            if (isUseCache)
            {
                if (File.Exists(pPath))
                {
                    return File.ReadAllBytes(pPath);
                }
            }

            var req = new UnityEngine.Networking.UnityWebRequest(ssPath);
            req.downloadHandler = new UnityEngine.Networking.DownloadHandlerFile(pPath);
            req.SendWebRequest();
            while (!req.isDone)
            {
                // Ignored
            }

            if (req.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + req.error);
                throw new System.Exception(req.error);
            }

            return File.ReadAllBytes(pPath);
#endif
        }


        public static string ReadAllText(string streamingAssetPath, bool isUseCache = false)
        {
            return Encoding.UTF8.GetString(ReadAllBytes(streamingAssetPath, isUseCache));
        }


        public static void SaveToStreamingAsset(string streamingAssetPath, byte[] data)
        {
            var path = PathUtils.GetStreamingAssetFullPath(streamingAssetPath);
            File.WriteAllBytes(path, data);
        }

        public static void SaveToStreamingAsset(string streamingAssetPath, string data)
        {
            var path = PathUtils.GetStreamingAssetFullPath(streamingAssetPath);
            File.WriteAllText(path, data);
        }
    }
}