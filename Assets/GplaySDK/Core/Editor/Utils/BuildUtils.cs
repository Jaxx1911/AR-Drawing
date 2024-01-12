// Filename: BuildUtils.cs
// Purpose: Use for Jenkins build process.
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 9:04 AM 23/10/2023
// 
// Notes: This will create an static method to call from CLI to build file.
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.IO;
using System.Linq;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.Core.Editor.BaseLib;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using EditorBuildSettings = UnityEditor.EditorBuildSettings;

namespace GplaySDK.Core.Editor.Utils
{
    public static class BuildUtils
    {
        private const string CancelToken = "565e580f7971524470f9ac203ec57d7eb0cacde1";

        private const AndroidSdkVersions SDKApiLevel = (AndroidSdkVersions) 33;

        private static class Android
        {
            public static void SDKInitialize()
            {
                string envRootPath = Environment.GetEnvironmentVariable("Unity_Build_Lib_HOME");
                if (string.IsNullOrEmpty(envRootPath))
                {
                    Debug.LogError("Please set UNITY_EDITOR_BUILD_LIB environment variable to your build lib path");
                    if (!Application.isBatchMode) return;
                    EditorApplication.Exit(1);
                    return;
                }

                envRootPath = Path.Combine(envRootPath, "Android");

                Debug.Log("Build Lib Path: " + envRootPath);
                //JDK JdkPath
                Debug.Log($"Current JDK Path: {EditorPrefs.GetString("JdkPath")}");
                string envJdkPath = Path.Combine(envRootPath, "JDK");
                if (!string.IsNullOrEmpty(envJdkPath))
                {
                    EditorPrefs.SetString("JdkPath", envJdkPath);
                    EditorPrefs.SetInt("JdkUseEmbedded", 0);
                }

                //SDK AndroidSdkRoot
                Debug.Log($"Current Android SDK Path: {EditorPrefs.GetString("AndroidSdkRoot")}");
                string envAndroidSdkRoot = Path.Combine(envRootPath, "SDK");
                if (!string.IsNullOrEmpty(envAndroidSdkRoot))
                {
                    EditorPrefs.SetString("AndroidSdkRoot", envAndroidSdkRoot);
                    EditorPrefs.SetInt("SdkUseEmbedded", 0);
                }

                //NDK AndroidNdkRootR21D || Use Unity default NDK
                EditorPrefs.DeleteKey("AndroidNdkRootR21D");
                EditorPrefs.SetInt("NdkUseEmbedded", 1);


                //Gradle GradlePath
                Debug.Log($"Current Gradle Path: {EditorPrefs.GetString("GradlePath")}");
                string envGradlePath = Path.Combine(envRootPath, "Gradle");
                if (!string.IsNullOrEmpty(envGradlePath))
                {
                    EditorPrefs.SetString("GradlePath", envGradlePath);
                    EditorPrefs.SetInt("GradleUseEmbedded", 0);
                }
            }

            public static void KeyStoreInitialize()
            {
                var coreConfig = GplaySDKConfig.GetConfig();
                PlayerSettings.Android.keystoreName = coreConfig.keyStorePath;
                PlayerSettings.Android.keystorePass = coreConfig.keyStorePass;
                PlayerSettings.Android.keyaliasName = coreConfig.keyAliasName;
                PlayerSettings.Android.keyaliasPass = coreConfig.keyAliasPass;
                PlayerSettings.Android.useCustomKeystore = true;
            }

            public static void _BuildFast(BuildInfo buildInfo)
            {
                Debug.Log("Run Fast Android Build");
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
                PlayerSettings.Android.targetSdkVersion = SDKApiLevel;
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
                EditorUserBuildSettings.buildAppBundle = false;
                _MainBuild(buildInfo);
            }

            public static void _BuildTest(BuildInfo buildInfo)
            {
                Debug.Log("Run Android Build Test");
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
                PlayerSettings.Android.targetSdkVersion = SDKApiLevel;
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                EditorUserBuildSettings.buildAppBundle = false;
                _MainBuild(buildInfo);
            }

            //_BuildAPK which il2cpp and 2 architecture
            public static void _BuildAPK(BuildInfo buildInfo)
            {
                Debug.Log("Run Android Build APK");
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
                PlayerSettings.Android.targetSdkVersion = SDKApiLevel;
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                EditorUserBuildSettings.buildAppBundle = false;
                _MainBuild(buildInfo);
            }

            //_BuildAAB which il2cpp and 2 architecture and build app bundle
            public static void _BuildAAB(BuildInfo buildInfo)
            {
                Debug.Log("Run Android Build AAB");
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
                PlayerSettings.Android.targetSdkVersion = SDKApiLevel;
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                EditorUserBuildSettings.buildAppBundle = true;
                _MainBuild(buildInfo);
            }

            private static void _MainBuild(BuildInfo buildInfo)
            {
                Editor_Initializer.InitializeAll();
                BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray(),
                    locationPathName = Path.Combine(buildInfo.buildPath, buildInfo.buildName),
                    target = BuildTarget.Android
                };

                var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
                if (report.summary.result != BuildResult.Succeeded)
                {
                    Debug.Log($"Build Fail - Cancel Token: {CancelToken}");
                    if (!Application.isBatchMode) return;
                    EditorApplication.Exit(1);
                }
            }
        }

        private static class Base
        {
            private const string IconPath = "GplaySDK/Core/Icon/{0}.png";
            private const string ActiveIconPath = "GplaySDK/Core/Icon/Active/ActiveIcon.png";

            public static void AssignBase()
            {
                bool isChanged = false;
                var coreConfig = GplaySDKConfig.GetConfig();
                // Update Package Name
                if (String.Compare(coreConfig.PackageName, PlayerSettings.applicationIdentifier,
                        StringComparison.Ordinal) != 0)
                {
                    PlayerSettings.applicationIdentifier = coreConfig.PackageName;
                    isChanged = true;
                }

                // Update Version Name
                if (String.Compare(coreConfig.versionName, PlayerSettings.bundleVersion,
                        StringComparison.Ordinal) != 0)
                {
                    PlayerSettings.bundleVersion = coreConfig.versionName;
                    isChanged = true;
                }

                // Update Logo
                if (coreConfig.Icon != null)
                {
                    PlayerSettings.SetIcons(NamedBuildTarget.Android, new Texture2D[] { coreConfig.Icon }, IconKind.Any);
                    PlayerSettings.SetIcons(NamedBuildTarget.iOS, new Texture2D[] { coreConfig.Icon }, IconKind.Any);
                }

                coreConfig.SaveToLocal();
                AssetDatabase.Refresh();
                if (isChanged) "Base Setting Changed...".Log();
            }


            [MenuItem("LA/Build/BuildConfig")]
            public static void BuildConfig()
            {
                AssignBase();
#if UNITY_ANDROID
                Android.SDKInitialize();
                Android.KeyStoreInitialize();
#endif
            }
        }


        private static class Utils
        {
            public static void SwapAsset(string source, string target)
            {
                $"Using: {source}".Log();
                if (File.Exists(target))
                {
                    "Target file already exist. Replace".Log();
                    FileUtil.ReplaceFile(source, target);
                }
                else
                {
                    "Target file not exist. Copy".Log();
                    FileUtil.CopyFileOrDirectory(source, target);
                }
            }
        }

        #region Jenkins

        public static void Jenkins_BuildFast(BuildInfo buildInfo)
        {
            Base.BuildConfig();
#if UNITY_ANDROID
            Android._BuildFast(buildInfo);
#endif
        }

        public static void Jenkins_BuildTest(BuildInfo buildInfo)
        {
            Base.BuildConfig();
#if UNITY_ANDROID
            Android._BuildTest(buildInfo);
#endif
        }

        public static void Jenkins_BuildAPK(BuildInfo buildInfo)
        {
            Base.BuildConfig();
#if UNITY_ANDROID
            Android._BuildTest(buildInfo);
#endif
        }

        public static void Jenkins_BuildAAB(BuildInfo buildInfo)
        {
            Base.BuildConfig();
#if UNITY_ANDROID
            Android._BuildAPK(buildInfo);
#endif
        }

        public static void JenkinsBuild()
        {
            var buildInfoPath =
                Path.Combine(Path.GetDirectoryName(Application.dataPath)!, "Jenkins", "build_info.json");
            Debug.Log(buildInfoPath);
            if (!File.Exists(buildInfoPath))
            {
                Debug.Log(
                    $"build_info.json not found in: {buildInfoPath}\nPlease check again.\n"
                    + "WARN: This script only for Jenkins invoke through buildUtils.py");
                if (Application.isBatchMode)
                {
                    EditorApplication.Exit(1);
                }

                return;
            }

            var buildInfoString = File.ReadAllText(buildInfoPath);
            var buildInfo = buildInfoString.FromJson<BuildInfo>();
            if (buildInfo is null)
            {
                Debug.Log("build_info.json is broken. Please check again...");
                if (Application.isBatchMode)
                {
                    EditorApplication.Exit(1);
                }

                return;
            }

            //Switch to target platform
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);


            //Main Script
            switch (buildInfo.buildType)
            {
                case "Fast":
                    Jenkins_BuildFast(buildInfo);
                    break;
                case "Test":
                    Jenkins_BuildTest(buildInfo);
                    break;
                case "APK":
                    Jenkins_BuildAPK(buildInfo);
                    break;
                case "AAB":
                    Jenkins_BuildAAB(buildInfo);
                    break;
                default:
                    Debug.Log("Unknown build type. Please check again...");
                    if (Application.isBatchMode)
                    {
                        EditorApplication.Exit(1);
                    }

                    return;
            }
        }

        [Serializable]
        public class BuildInfo
        {
            public string buildPath;
            public string buildName;
            public string timestamp;
            public string unityVersion;
            public string platform;
            public string buildType;
        }

        #endregion
    }
}