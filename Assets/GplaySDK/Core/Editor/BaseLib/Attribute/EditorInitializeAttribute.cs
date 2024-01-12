// Filename: EditorInitializeAttribute.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 1:44 PM 05/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using JetBrains.Annotations;

namespace GplaySDK.Core.Editor.BaseLib.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse]
    public class EditorInitializeAttribute : System.Attribute
    {
        public int Order { get; private set; }

        public EditorInitializeLoadType LoadType { get; private set; }

        public EditorInitializeAttribute(int order, EditorInitializeLoadType loadType)
        {
            Order = order;
            LoadType = loadType;
        }
    }
}