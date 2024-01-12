// Filename: RuntimeInitializerAttribute.cs
// Purpose: To determine the order of class initialization. Also used to mark the class that needs to be initialized.
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:28 PM 21/09/2023
// 
// Notes: smaller order will be initialized first
//        if order is the same, the class will be initialized in alphabetical order
//        if order is not set, the class will be initialized last
//        
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GplaySDK.Core.BaseLib.Attribute
{
    [AttributeUsage(AttributeTargets.Method)][MeansImplicitUse]
    public class RuntimeInitializeAttribute : System.Attribute
    {
        public int Order { get; private set; }
        
        public RuntimeInitializeLoadType LoadType { get; private set; }

        public RuntimeInitializeAttribute(int order,
            RuntimeInitializeLoadType loadType)
        {
            Order = order;
            LoadType = loadType;
        }
    }
}