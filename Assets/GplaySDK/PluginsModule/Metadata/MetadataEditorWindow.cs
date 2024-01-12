// Filename: MetadataEditorWindow.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 17:34 13/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace GplaySDK.Metadata
{
    public class MetadataEditorWindow : OdinEditorWindow
    {
        [ShowInInspector, TableList]
        private Dictionary<string, Metadata> Metadata
        {
            get => MetadataController.Metadata;
            // ReSharper disable once ValueParameterNotUsed
            set
            {
                //Ignore
            }
        }


        [MenuItem("LA/Metadata/Open Metadata Window")]
        private static void OpenWindow()
        {
            CreateInstance<MetadataEditorWindow>().Show();
        }

        [Button]
        private static void ScanProject()
        {
            MetadataController.ScanProject();
        }

        [Button]
        private static void RemoveAllMetadata()
        {
            MetadataController.RemoveAllMetadata();
        }


        [Button]
        private static void CheckDuplicate()
        {
            MetadataController.CheckDuplicate();
        }


        [Button]
        private static void UploadToDatabase()
        {
            MetadataController.UploadAllToDatabase();
        }
    }
}