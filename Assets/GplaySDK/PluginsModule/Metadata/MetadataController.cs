// Filename: MetadataController.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 17:16 13/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GplaySDK.Core;
using GplaySDK.Core.BaseLib.Const;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.Network;

namespace GplaySDK.Metadata
{
    internal static class MetadataController
    {
        private static readonly string[] AllowExtension =
        {
            "png", "jpg", "jpeg", "tga", "tif", "tiff", "gif", "psd", "bmp", "json", "asset"
        };

        private static readonly string[] ImageExtension =
            { "png", "jpg", "jpeg", "tga", "tif", "tiff", "gif", "psd", "bmp" };


        private static readonly string[] SkipPath =
        {
            "Assets/GplaySDK",
            "Assets/Plugins",
            "Assets/Spine",
            "Assets/AddressableAssetsData",
            "Assets/MyTools/Unity-Logs-Viewer",
            "Assets/GoogleMobileAds",
            "Assets/MaxSdk",
            "Assets/Editor/SpineSettings.asset",
            "Assets/StreamingAssets/google-services-desktop.json",
            "Assets/google-services.json",
            "Assets/Packages/",
            "Assets/NuGet/"
        };


        internal static readonly Dictionary<string, Metadata> Metadata = new Dictionary<string, Metadata>();

        public static void ScanProject()
        {
            Metadata.Clear();

            ScanFolder("Assets");
        }

        private static void ScanFolder(string path, string type = "*", bool recursive = true)
        {
            $"Current path: {Path.GetFullPath(path)}".Log();
            var files = Directory.GetFiles(path, type,
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                if (SkipByExtension(Path.GetExtension(file)))
                {
                    continue;
                }

                if (SkipByPath(file))
                {
                    continue;
                }

                var fileInfo = new FileInfo(file);
                var metadata = new Metadata
                {
                    name = fileInfo.Name,
                    path = file,
                    hash = CalculateHash(Path.GetFullPath(file)),
                    size = fileInfo.Length
                };
                Metadata.Add(file, metadata);
            }
        }

        private static bool SkipByExtension(string extension)
        {
            if (extension.StartsWith(".")) extension = extension.Substring(1);

            extension = extension.ToLower();

            return !AllowExtension.Contains(extension);
        }

        private static bool SkipByPath(string path)
        {
            return SkipPath.Any(path.StartsWith);
        }

        private static string CalculateHash(string filePath)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            using var stream = File.OpenRead(filePath);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public static async void CheckDuplicate()
        {
            var checkData = new CheckRequest()
            {
                aliasId = GplaySDKConfig.GetConfig().baseLibConfig.projectSdkAliasId,
                checkList = Metadata.Select(pair => pair.Value.hash).ToArray()
            };
            var checkDataJson = await checkData.ToJsonAsync();
            $"Check duplicate: \n{checkDataJson}".Log();
            string response = await NetworkUtils.PostAsync(StringConst.Editor.SdkServer.Url.Metadata.CheckDuplicate, checkDataJson);

            var responseContent = await response.ToJsonAsync(true);
            $"Check duplicate response: \n{responseContent}".Log();
        }

        public static async void UploadAllToDatabase()
        {
            var baseConfig = GplaySDKConfig.GetConfig();
            foreach (var metadata in Metadata.Values)
            {
                await UploadToDatabase(metadata, baseConfig);
            }
        }

        public static async Task UploadToDatabase(Metadata metadata, GplaySDKConfig baseConfig)
        {
            if (IsImage(metadata.path))
            {
                RemoveMetadata(metadata.path);
            }

            var addRequest = new AddRequest(baseConfig, metadata);
            var response = (await NetworkUtils.PostAsync(StringConst.Editor.SdkServer.Url.Metadata.AddOrUpdate, await addRequest.ToJsonAsync()))
                .FromJson<AddResponse>();
            if (response.isSuccess)
            {
                $"Add metadata success: {metadata.name}\n{response.id}\n{response.message}".Log();
            }
            else
            {
                $"Add metadata failed: {metadata.name}\n{response.message}".LogError();
            }
        }

        public static void RemoveAllMetadata()
        {
            foreach (var metadata in Metadata.Values)
            {
                if (IsImage(metadata.path))
                    RemoveMetadata(metadata.path);
            }
        }

        public static void RemoveMetadata(string path)
        {
            //Read All metadata of image in path
            using var image = Image.FromFile(path);
            var metadata = image.PropertyItems;
            foreach (var propertyItem in metadata)
            {
                image.RemovePropertyItem(propertyItem.Id);
            }

            image.Save(path);
        }

        private static bool IsImage(string path)
        {
            var extension = Path.GetExtension(path);
            if (extension.StartsWith(".")) extension = extension.Substring(1);

            extension = extension.ToLower();

            return ImageExtension.Contains(extension);
        }

        [Serializable]
        private class CheckRequest
        {
            public string aliasId;
            public string[] checkList;
        }

        [Serializable]
        private class AddRequest
        {
            public string aliasId;
            public string projectVersion;
            public string name;
            public string path;
            public string hash;
            public long size;

            public AddRequest(GplaySDKConfig baseConfig, Metadata baseMetadata)
            {
                aliasId = baseConfig.baseLibConfig.projectSdkAliasId;
                projectVersion = $"{baseConfig.baseLibConfig.gitBranch}_{baseConfig.baseLibConfig.gitCommitId}";
                name = baseMetadata.name;
                path = baseMetadata.path;
                hash = baseMetadata.hash;
                size = baseMetadata.size;
            }
        }

        [Serializable]
        private class AddResponse
        {
            public string message;
            public bool isSuccess;
            public string id;
        }
    }
}