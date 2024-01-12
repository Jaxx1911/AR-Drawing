// Filename: GplaySDKConfig.cs
// Purpose: Store all config for GplaySDK
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:19 PM 21/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.IO;
using System.Text.RegularExpressions;
using GplaySDK.Core.BaseLib;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.MediationIntegration.BaseLib;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable RedundantDefaultMemberInitializer
namespace GplaySDK.Core
{
    public sealed class GplaySDKConfig : SerializedScriptableObject
    {
        #region GplaySDK Config

        [TitleGroup("GplaySDK Config", Order = 0)]

        #region Compile Config

        [FoldoutGroup("GplaySDK Config/Compile Config", Order = 1)]
        [SerializeField, LabelText("Log Verbose"), PropertyOrder(0)]
        internal bool logVerbose = false;

        [FoldoutGroup("GplaySDK Config/Compile Config")] [SerializeField, LabelText("Log Error"), PropertyOrder(1)]
        internal bool logError = false;

        [FoldoutGroup("GplaySDK Config/Compile Config")] [SerializeField, LabelText("Tester"), PropertyOrder(2)]
        internal bool tester = false;

        [FoldoutGroup("GplaySDK Config/Compile Config")] [SerializeField, LabelText("Test Server"), PropertyOrder(3)]
        internal bool testServer = false;

        [FoldoutGroup("GplaySDK Config/Compile Config")] [SerializeField, LabelText("Firebase Only"), PropertyOrder(4)]
        internal bool firebaseOnly = false;

        [FoldoutGroup("GplaySDK Config/Compile Config")] [SerializeField, LabelText("No Sync"), PropertyOrder(5)]
        internal bool noSync = false;

        #endregion

        // ReSharper disable once UnassignedField.Global
        [TitleGroup("GplaySDK Config"), LabelText("Force Update Prefab")]
        public IPopup ForceUpdatePrefab;

        #region Project Config

        #region Project Build Name

        [TitleGroup("Project Config", Order = 1)]
        [FoldoutGroup("Project Config/Project Build Name", 0)]
        [ShowInInspector, LabelText("Project Build Name"), PropertyOrder(0)]
        public string projectBuildName
        {
            get
            {
#if UNITY_ANDROID
                return projectBuildNameAndroid;
#elif UNITY_IOS
                return projectBuildNameIOS;
#endif
            }
        }

        [SerializeField, LabelText("Project Build Name Android"), PropertyOrder(1),
         FoldoutGroup("Project Config/Project Build Name")]
        private string projectBuildNameAndroid;

        [SerializeField, LabelText("Project Build Name IOS"), PropertyOrder(2),
         FoldoutGroup("Project Config/Project Build Name")]
        private string projectBuildNameIOS;

        #endregion

        #region Package Name

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Project Config/Package Name", 2)]
        public string PackageName
        {
            get
            {
#if UNITY_ANDROID
                return packageNameAndroid;
#elif UNITY_IOS
                return packageNameIOS;
#endif
            }
        }

        [SerializeField, LabelText("Package Name Android"), PropertyOrder(1),
         FoldoutGroup("Project Config/Package Name")]
        private string packageNameAndroid;

        [SerializeField, LabelText("Package Name IOS"), PropertyOrder(2),
         FoldoutGroup("Project Config/Package Name")]
        private string packageNameIOS;

        [LabelText("Ios App Id"), PropertyOrder(3), FoldoutGroup("Project Config/Package Name")]
        public string iosAppId;

        #endregion

        #region Icon

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Project Config/Icon Path", 3)]
        public Texture2D Icon
        {
            get
            {
#if UNITY_ANDROID
                return iconAndroid;
#elif UNITY_IOS
                return iconIOS;
#endif
            }
        }

        [SerializeField, LabelText("Icon Android"), PropertyOrder(1), FoldoutGroup("Project Config/Icon Path")]
        private Texture2D iconAndroid;
        
        [SerializeField, LabelText("Icon IOS"), PropertyOrder(2), FoldoutGroup("Project Config/Icon Path")]
        private Texture2D iconIOS;

        #endregion

        #region Other Link

        [ShowInInspector, ReadOnly, PropertyOrder(0), FoldoutGroup("Project Config/Other Link", 4)]
        public string OpenLinkRate
        {
            get
            {
#if UNITY_ANDROID
                return "market://details?id=" + packageNameAndroid;
#elif UNITY_IOS
                return "itms-apps://itunes.apple.com/app/id" + iosAppId;
#endif
            }
        }

        [ShowInInspector, ReadOnly, PropertyOrder(1), FoldoutGroup("Project Config/Other Link")]
        public string FanPageLinkWeb
        {
            get
            {
#if UNITY_ANDROID
                return "https://www.facebook.com/abc";
#elif UNITY_IOS
                return "https://www.facebook.com/abc";
#endif
            }
        }


        [ShowInInspector, ReadOnly, PropertyOrder(2), FoldoutGroup("Project Config/Other Link")]
        public string FanPageLinkApp
        {
            get
            {
#if UNITY_ANDROID
                return "fb://page/abc";
#elif UNITY_IOS
                return "fb://page/abc";
#endif
            }
        }

        [ShowInInspector, ReadOnly, PropertyOrder(3), FoldoutGroup("Project Config/Other Link")]
        public string FeedbackLink
        {
            get
            {
#if UNITY_ANDROID
                return "support@mkt.gplayjsc.com";
#elif UNITY_IOS
                return "support@mkt.gplayjsc.com";
#endif
            }
        }

        [ShowInInspector, ReadOnly, PropertyOrder(4), FoldoutGroup("Project Config/Other Link")]
        public string PolicyLink
        {
            get
            {
#if UNITY_ANDROID
                return "https://sites.google.com/view/global-playstudio-policy";
#elif UNITY_IOS
                return "https://sites.google.com/view/global-playstudio-policy";
#endif
            }
        }

        [ShowInInspector, ReadOnly, PropertyOrder(5), FoldoutGroup("Project Config/Other Link")]
        public string TermOfUseLink
        {
            get
            {
#if UNITY_ANDROID
                return "https://sites.google.com/view/global-play-terms-of-use";
#elif UNITY_IOS
                return "https://sites.google.com/view/global-play-terms-of-use";
#endif
            }
        }

        [ShowInInspector, ReadOnly, PropertyOrder(6), FoldoutGroup("Project Config/Other Link")]
        public string MoreGameLink
        {
            get
            {
#if UNITY_ANDROID
                return "https://play.google.com/store/apps/dev?id=5898471006814482851";
#elif UNITY_IOS
                return "https://apps.apple.com/us/developer/tu-nguyen-ngoc/id1646918801";
#endif
            }
        }

        #endregion

        #region Version

        [PropertyOrder(0), FoldoutGroup("Project Config/Version", 5)]
        public int versionCode = 2023010101;

        [PropertyOrder(1), FoldoutGroup("Project Config/Version")]
        public string versionName = "1.0.0";

        #endregion

        #region Protech Data

        #region Key Store

        [ProtectedSerialize]
        [PropertyOrder(0), FoldoutGroup("Project Config/Key Store", 6)]
        [FilePath(AbsolutePath = false, Extensions = "keystore", ParentFolder = "")]
        public string keyStorePath = "";

        [ProtectedSerialize] [PropertyOrder(1), FoldoutGroup("Project Config/Key Store")]
        public string keyStorePass = "gplayjsc";

        [ProtectedSerialize] [PropertyOrder(2), FoldoutGroup("Project Config/Key Store")]
        public string keyAliasName = "gplayjsc";

        [ProtectedSerialize] [PropertyOrder(3), FoldoutGroup("Project Config/Key Store")]
        public string keyAliasPass = "gplayjsc";

        #endregion

        [ProtectedSerialize] [PropertyOrder(0), FoldoutGroup("Project Config/IAP Android Hash", 7)]
        public string iapAndroidKeyHash = "hash";

        #endregion

        #endregion


        [TitleGroup("Unsafe Config", Order = 2)]
        public BaseLibConfig baseLibConfig;

        #endregion

        #region Ads Config

        [TitleGroup("Ads Integration", Order = 3)]
        public BaseAdsConfig baseAdsConfig;

        #endregion


        #region Admob Integration

#if GplaySDK_AdmobIntegration
        [TitleGroup("Ads Integration")] public AdmobIntegration.AdMobConfig adMobConfig;
#endif

        #endregion

        #region Ironsource Integration

#if GplaySDK_IronSourceIntegration
        [TitleGroup("Ads Integration")]public GplaySDK.IronSourceIntegration.IronSourceConfig ironSourceConfig;
#endif

        #endregion

        #region Max Integration

#if GplaySDK_MaxIntegration
        [TitleGroup("Ads Integration")] public MaxIntegration.MaxConfig maxConfig;
#endif

        #endregion


        public static GplaySDKConfig GetConfig()
        {
            var instant = Resources.Load<GplaySDKConfig>("GplaySDKConfig");
#if UNITY_EDITOR
            if (instant == null)
            {
                Directory.CreateDirectory("Assets/GplaySDK/Resources");
                //Create new instant
                instant = CreateInstance<GplaySDKConfig>();
                UnityEditor.AssetDatabase.CreateAsset(instant, "Assets/GplaySDK/Resources/GplaySDKConfig.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
                instant._Initialize();
            }
#else
            if (instant == null)
                throw new FileNotFoundException("GplaySDKConfig.asset not found");
#endif
            return instant;
        }

#if UNITY_EDITOR
        public void SaveToLocal()
        {
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            //Call SaveToLocal For all BaseConfig which include
            baseLibConfig.SaveToLocal();
            baseAdsConfig.SaveToLocal();
#if GplaySDK_AdmobIntegration
            adMobConfig.SaveToLocal();
#endif
#if GplaySDK_IronSourceIntegration
            ironSourceConfig.SaveToLocal();
#endif
#if GplaySDK_MaxIntegration
            maxConfig.SaveToLocal();
#endif
        }

        [UnityEditor.InitializeOnLoadMethod]
        private static void InitializeGitInfo()
        {
            var config = GetConfig();

            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("git")
            {
                UseShellExecute = false,
                WorkingDirectory = Application.dataPath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                Arguments = "rev-parse --abbrev-ref HEAD"
            };

            System.Diagnostics.Process getBranchNameProcess = new System.Diagnostics.Process();
            getBranchNameProcess.StartInfo = startInfo;
            getBranchNameProcess.Start();

            startInfo.Arguments = "rev-parse HEAD";
            System.Diagnostics.Process getCommitIdProcess = new System.Diagnostics.Process();
            getCommitIdProcess.StartInfo = startInfo;
            getCommitIdProcess.Start();

            config.baseLibConfig.gitBranch = getBranchNameProcess.StandardOutput.ReadLine();
            config.baseLibConfig.gitCommitId = getCommitIdProcess.StandardOutput.ReadLine();

            $"Git Branch: {config.baseLibConfig.gitBranch}\nGit Commit Id: {config.baseLibConfig.gitCommitId}".Log();

            config.SaveToLocal();
        }


#endif

        [Button("Force Initialize")]
        [PropertyOrder(Order = 1000)]
        [RuntimeInitialize(-10000, RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        private static void Initialize()
        {
            var instant = GetConfig();
            instant._Initialize();
        }

        private void _Initialize()
        {
            baseLibConfig = BaseLibConfig.Initialize(baseLibConfig);
            baseAdsConfig = BaseAdsConfig.Initialize(baseAdsConfig);
#if GplaySDK_AdmobIntegration
            adMobConfig = AdmobIntegration.AdMobConfig.Initialize(adMobConfig);
#endif
#if GplaySDK_IronSourceIntegration
            //TODO: Write IronSource Integration later
            //GplaySDK.IronSourceIntegration.IronSourceConfig.Initialize(ironSourceConfig);
#endif
#if GplaySDK_MaxIntegration
            maxConfig = MaxIntegration.MaxConfig.Initialize(maxConfig);
#endif
        }

        [Button("UnPlug All Config Module")]
        [PropertyOrder(Order = 1000)]
        private void UnPlugAllConfig()
        {
            baseLibConfig = null;
            baseAdsConfig = null;
#if GplaySDK_AdmobIntegration
            adMobConfig = null;
#endif
#if GplaySDK_IronSourceIntegration
            ironSourceConfig = null;
#endif
#if GplaySDK_MaxIntegration
            maxConfig = null;
#endif
        }

#if UNITY_EDITOR

        [Button("Import Setting")]
        [PropertyOrder(Order = 1000)]
        private async void ImportSetting()
        {
            var basicConfig = await LocalServer.LocalServerController.GetBasicSdkConfig();
            if (basicConfig != null)
            {
                if (basicConfig.IsAndroid())
                {
                    projectBuildNameAndroid = basicConfig.name;
                    packageNameAndroid = basicConfig.packageName;
                }
                else if (basicConfig.IsIos())
                {
                    projectBuildNameIOS = basicConfig.name;
                    packageNameIOS = basicConfig.packageName;
                }
                else
                {
                    "Not found storeType".LogError();
                }
            }
            else
            {
                "Get basic config failed".LogError();
            }
#if GplaySDK_MaxIntegration
            await maxConfig.GetConfigFromLocalServer();
#endif
#if GplaySDK_AdmobIntegration
            await adMobConfig.GetConfigFromLocalServer();
#endif
            SaveToLocal();
        }

        private void OnValidate()
        {
            InitializeSymbol();
        }


        public void InitializeSymbol()
        {
            var currentTarget = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
            var currentGroup = UnityEditor.BuildPipeline.GetBuildTargetGroup(currentTarget);
            string allSymbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(currentGroup) ?? "";
            if (logVerbose)
            {
                if (!allSymbols.Contains("LOG_VERBOSE"))
                {
                    allSymbols += ";LOG_VERBOSE";
                }
            }
            else
            {
                allSymbols = allSymbols.Replace("LOG_VERBOSE", "");
            }

            if (logError)
            {
                if (!allSymbols.Contains("LOG_ERROR"))
                {
                    allSymbols += ";LOG_ERROR";
                }
            }
            else
            {
                allSymbols = allSymbols.Replace("LOG_ERROR", "");
            }

            if (tester)
            {
                if (!allSymbols.Contains("TESTER"))
                {
                    allSymbols += ";TESTER";
                }
            }
            else
            {
                allSymbols = allSymbols.Replace("TESTER", "");
            }

            if (testServer)
            {
                if (!allSymbols.Contains("TEST_SERVER"))
                {
                    allSymbols += ";TEST_SERVER";
                }
            }
            else
            {
                allSymbols = allSymbols.Replace("TEST_SERVER", "");
            }

            if (firebaseOnly)
            {
                if (!allSymbols.Contains("FIREBASE_ONLY"))
                {
                    allSymbols += ";FIREBASE_ONLY";
                }
            }
            else
            {
                allSymbols = allSymbols.Replace("FIREBASE_ONLY", "");
            }

            if (noSync)
            {
                if (!allSymbols.Contains("NO_SYNC"))
                {
                    allSymbols += ";NO_SYNC";
                }
            }
            else
            {
                allSymbols = allSymbols.Replace("NO_SYNC", "");
            }

            if (baseLibConfig != null)
            {
                baseLibConfig.InitializeSymbol(ref allSymbols);
            }


            allSymbols = Regex.Replace(allSymbols, @";+", ";");
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(currentGroup, allSymbols);
        }
#endif
    }
}