// Filename: StringHelper.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:46 PM 16/10/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using GplaySDK.Core.BaseLib.Attribute;

namespace GplaySDK.Core.BaseLib.Const
{
    public static class StringConst
    {
        public static class LocalKey
        {
            public static class Common
            {
                public const string Is_First_Open = "is_first_open";
            }

            public static class Ads
            {
                public const string Number_Ads_Today = "number_ads_today";
                public const string Number_Ads_This_Session = "number_ads_this_session";
            }

            public static class Time
            {
                public const string First_Open_Time = "first_open_time";
            }

            public static class SKan
            {
                public const string First_Day_Revenue = "first_day_revenue";
            }
        }

        public static class RemoteKey
        {
            public static class ForceUpdate
            {
                [RemoteKey] public const string Remote_Key = "force_update_major_version";
            }

            public static class SKan
            {
                [RemoteKey] public const string Remote_Key = "skan_remote_schema";
            }

            public static class Segment
            {
                public static class Banner
                {
                    [RemoteKey] public const string MinimumLevel = "banner_minimum_level";
                    [RemoteKey] public const string HideOnScenes = "banner_hide_on_scenes";
                }

                public static class Interstitial
                {
                    [RemoteKey] public const string MinimumLevel = "interstitial_minimum_level";
                    [RemoteKey] public const string Cooldown = "interstitial_cooldown";
                }
            }
        }
#if UNITY_EDITOR
        public static class Editor
        {
            public static class SdkServer
            {
                internal static class Prefix
                {
                    internal const string SdkBaseUrl = "http://192.168.1.70:8000/{0}/{1}";

                    internal static class Metadata
                    {
                        internal const string ApiPrefix = "metadata";

                        internal const string CheckDuplicate = "check";

                        internal const string AddOrUpdate = "add_or_update";
                    }

                    internal static class ProjectId
                    {
                        internal const string ApiPrefix = "project-id";

                        internal const string GetBasicData = "get_basic_data";

                        internal const string GetMaxSdkConfig = "get_max_sdk_config";

                        internal const string GetAdmobConfig = "get_admob_config";
                    }
                }

                public static class Url
                {
                    public static class Metadata
                    {
                        public static readonly string CheckDuplicate = string.Format(Prefix.SdkBaseUrl,
                            Prefix.Metadata.ApiPrefix, Prefix.Metadata.CheckDuplicate);

                        public static readonly string AddOrUpdate = string.Format(Prefix.SdkBaseUrl,
                            Prefix.Metadata.ApiPrefix,
                            Prefix.Metadata.AddOrUpdate);
                    }

                    public static class Core
                    {
                        public static readonly string GetBasicData = string.Format(Prefix.SdkBaseUrl,
                            Prefix.ProjectId.ApiPrefix, Prefix.ProjectId.GetBasicData);

                        public static readonly string GetMaxData = string.Format(Prefix.SdkBaseUrl,
                            Prefix.ProjectId.ApiPrefix, Prefix.ProjectId.GetMaxSdkConfig);

                        public static readonly string GetAdmobData = string.Format(Prefix.SdkBaseUrl,
                            Prefix.ProjectId.ApiPrefix, Prefix.ProjectId.GetAdmobConfig);
                    }
                }
            }
        }
#endif
    }
}