// Filename: BannerZone.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 14:48 8/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using GplaySDK.BaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace GplaySDK.MediationIntegration.BaseLib
{
    [RequireComponent(typeof(RectTransform),typeof(Image))]
    public class BannerZone: MonoBehaviour
    {
        public Vector2 GetSize()
        {
            var rect = GetComponent<RectTransform>();
            return rect.sizeDelta;
        }

    }
}