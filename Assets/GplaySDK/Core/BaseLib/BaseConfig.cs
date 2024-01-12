// Filename: BaseConfig.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:22 AM 17/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.IO;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GplaySDK.Core.BaseLib
{
    public abstract class BaseConfig<TSelf> : SerializedScriptableObject where TSelf : BaseConfig<TSelf>
    {
        [FilePath(AbsolutePath = false)] protected abstract string ConfigName { get; }

        private string configPath => "Assets/GplaySDK/Resources/_C_" + ConfigName + ".asset";


        /// <summary>
        /// Initialize static config and create instance if not exist.
        /// Will need to create an static method which call this method in each config class.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TSelf Initialize(TSelf config)
        {
            if (config != null)
            {
                return config._Initialize();
            }
#if UNITY_EDITOR
            //Create new instance if not exist
            var instance = CreateInstance<TSelf>();
            UnityEditor.AssetDatabase.CreateAsset(instance, instance.configPath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            return instance._Initialize();
#else
            throw new FileNotFoundException($"Config is null| {typeof(TSelf).FullName}");
#endif
        }

        protected abstract TSelf _Initialize();

#if UNITY_EDITOR
        public void SaveToLocal()
        {
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        public abstract Task GetConfigFromLocalServer();

#endif
    }
}