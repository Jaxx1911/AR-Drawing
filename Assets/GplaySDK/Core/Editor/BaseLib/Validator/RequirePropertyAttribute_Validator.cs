// Filename: RequirePropertyAttribute_Validator.cs
// Purpose: 
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 10:26 AM 29/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Linq;
using System.Reflection;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Exception;
using GplaySDK.Core.Editor.BaseLib.Attribute;
using Sirenix.Utilities;

namespace GplaySDK.Core.Editor.BaseLib.Validator
{
    internal static class RequirePropertyAttribute_Validator
    {
        [EditorInitialize(-1000, EditorInitializeLoadType.OnValidate)]
        private static void Validator()
        {
#if G_SKIP_REQUIRE_PROPERTY
            return;
#endif
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var allImplements = assemblies
                .SelectMany(assembly => assembly
                    .GetTypes()
                    .Where(filter =>
                        filter.IsClass
                        && !filter.IsAbstract
                        && filter.IsSubclassOf(typeof(RequirePropertyAttribute))))
                .ToList();

            var allProperties = assemblies
                .SelectMany(assembly => assembly
                    .GetTypes()
                    .SelectMany(type => type
                        .GetProperties(BindingFlags.Static
                                       | BindingFlags.Public
                                       | BindingFlags.NonPublic)
                        .Where(method => method
                            .GetCustomAttributes(typeof(RequirePropertyAttribute), false)
                            .Any())))
                .ToArray();
            foreach (var property in allProperties)
            {
                var requirePropertyAttribute = property.GetCustomAttribute<RequirePropertyAttribute>();
                if (!property.IsStatic())
                {
                    throw new RequirePropertyException("Property " + property.Name + " in class " +
                                                       property.PropertyType.Name +
                                                       " is not static. Please check again!");
                }

                if (!requirePropertyAttribute.ValidatorType(property.PropertyType))
                {
                    throw new RequirePropertyException("Property " + property.Name + " in class " +
                                                       property.PropertyType.Name +
                                                       " is not valid type. Please check again!");
                }

                //Check if property is only have one instance
                var propertyCount =
                    allProperties.Count(filter =>
                        filter.GetCustomAttribute<RequirePropertyAttribute>().Name == requirePropertyAttribute.Name);
                if (propertyCount > 1)
                {
                    throw new RequirePropertyException("Property " + property.Name + " in class " +
                                                       property.PropertyType.Name +
                                                       " is not singleton. Please check again!");
                }

                allImplements.Remove(requirePropertyAttribute.GetType());
            }


            if (allImplements.Count > 0)
            {
                // Still not implement all
                var extraValidateMessage = $"Not implement all RequirePropertyAttribute. Please check again!\n";
                foreach (var implement in allImplements)
                {
                    extraValidateMessage += implement.Name + "\n";
                }

                throw new RequirePropertyException(extraValidateMessage);
            }
        }
    }
}