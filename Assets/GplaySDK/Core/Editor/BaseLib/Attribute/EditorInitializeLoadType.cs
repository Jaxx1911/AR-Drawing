// Filename: EditorInitializeLoadType.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 1:45 PM 05/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

namespace GplaySDK.Core.Editor.BaseLib.Attribute
{
    public enum EditorInitializeLoadType
    {
        None = 0,
        OnValidate = 1,
        OnSymbolInitialize = 2,
        OnAsmdefInitialize = 3,
        OnCompileInitialize = 4,
    }
}