// Filename: Metadata.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 17:19 13/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using GplaySDK.Core;
using Sirenix.OdinInspector;

namespace GplaySDK.Metadata
{
    internal struct Metadata
    {
        public string name;
        public string path;
        public string hash;
        public long size;

        [Button]
        public void AddToDatabase()
        {
            var config = GplaySDKConfig.GetConfig();
            MetadataController.UploadToDatabase(this, config);
        }

        [Button]
        public void RemoveAllMetadata()
        {
            MetadataController.RemoveMetadata(path);
        }
    }
}