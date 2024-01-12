// Filename: IPopup.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 11:57 8/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.Core.BaseLib
{
    public interface IPopup
    {
        public void Show(Action onShow);
        public void Show(Action onShow, params object[] args);
    }
}