// Filename: RequirePropertyException.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 10:05 AM 29/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com


namespace GplaySDK.Core.BaseLib.Exception
{
    public class RequirePropertyException: System.Exception
    {
        public RequirePropertyException(string message) : base(message)
        {
        }
    }
}