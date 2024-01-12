// Filename: ProtectedSerializeAttribute.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 2:01 PM 06/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;

namespace GplaySDK.Core.BaseLib.Attribute
{
    //TODO: Write this attribute. Write a custom serializer for this attribute.
    //[Obsolete("This attribute is not completed yet",false)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ProtectedSerializeAttribute : System.Attribute
    {
        private ProtectType Behaviour { get; }


        public ProtectedSerializeAttribute(ProtectType behaviour = ProtectType.ClearOnBuild)
        {
            Behaviour = behaviour;
        }

        public enum ProtectType : int
        {
            None = 0,
            ClearOnBuild = 1,
            GlobalHash = 2
        }
        
    }
}