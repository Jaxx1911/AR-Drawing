// Filename: AdsController.cs
// Purpose: Wrapper for all ads network.
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 2:43 PM 22/09/2023
// 
// Notes: Business logic for ads. This class is a wrapper for all ads network.
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using System.Collections.Generic;
using GplaySDK.BaseLib.RequireProperty;
using GplaySDK.Core.BaseLib.Attribute;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.MediationIntegration.BaseLib;
using GplaySDK.MediationIntegration.BaseLib.AdDecider;
using GplaySDK.MediationIntegration.BaseLib.LoadInfo;
using GplaySDK.Segment;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GplaySDK.Ads
{
    public static class AdsController
    {
        private static DateTime _interstitialShowTime;

        private static DateTime _openAdsShowTime;

        private static BaseAdsConfig _baseAdsConfig;

        // ReSharper disable once RedundantDefaultMemberInitializer
        private static bool _isInitialized = false;

        #region Ad Decider

        private static BannerDecider _bannerAdDecider;

        private static InterstitialAdDecider _interstitialAdDecider;

        private static RewardedAdDecider _rewardedAdDecider;

        private static OpenAdsDecider _openAdsDecider;

        private static NativeAdDecider _nativeAdDecider;

        #endregion


        private static bool IsCanShowInterstitial
        {
            get
            {
                if (!_isInitialized)
                {
#if LOG_VERBOSE
                    "AdsController is not initialized".Log();
#endif
                    return false;
                }

                if (IsRemoveAdsAttribute.Value)
                {
#if LOG_VERBOSE
                    "Interstitial not show| Remove Ads".Log();
#endif
                    return false;
                }

                if (DateTime.Now <= _interstitialShowTime)
                {
#if LOG_VERBOSE
                    "Interstitial not show| On cooldown".Log();
#endif
                    return false;
                }

                // ReSharper disable once InvertIf
                if (CurrentLevelAttribute.Value <= MinimumLevelShowInterstitialAttribute.Value)
                {
#if LOG_VERBOSE
                    "Interstitial not show| Lower than minimum level".Log();
#endif
                    return false;
                }


                return true;
            }
        }

        private static bool IsCanShowOpenAds
        {
            get
            {
                if (!_isInitialized)
                {
#if LOG_VERBOSE
                    "AdsController is not initialized".Log();
#endif
                }

                if (IsRemoveAdsAttribute.Value)
                {
#if LOG_VERBOSE
                    "Open Ads not show| Remove Ads".Log();
#endif
                    return false;
                }

                if (DateTime.Now <= _openAdsShowTime)
                {
#if LOG_VERBOSE
                    "Open Ads not show| On cooldown".Log();
#endif
                    return false;
                }

                // ReSharper disable once InvertIf
                if (CurrentLevelAttribute.Value <= MinimumLevelShowInterstitialAttribute.Value)
                {
#if LOG_VERBOSE
                    "Open Ads not show| On Interstitial Cooldown".Log();
#endif
                    return false;
                }

                return true;
            }
        }


        #region SDK Integration

        // ReSharper disable once ArrangeObjectCreationWhenTypeEvident
        // ReSharper disable once UnusedMember.Local
        private static Dictionary<AdIntegrationType, BaseAdsIntegration> _loadedAdsIntegration =
            new Dictionary<AdIntegrationType, BaseAdsIntegration>();

        #endregion


        [RuntimeInitialize(-10, RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _baseAdsConfig = BaseAdsConfig.Instance;
            _openAdsShowTime = DateTime.Now.Add(TimeSpan.FromSeconds(-300));
            _interstitialShowTime = DateTime.Now.Add(TimeSpan.FromSeconds(-300));
            _bannerAdDecider = new BannerDecider();
            _interstitialAdDecider = new InterstitialAdDecider();
            _rewardedAdDecider = new RewardedAdDecider();
            _openAdsDecider = new OpenAdsDecider();
            _nativeAdDecider = new NativeAdDecider();
            _isInitialized = true;

            //Register Banner Show When Change Scene
            /*SceneManager.activeSceneChanged += (current, next) =>
            {
                var bannerSegment = SegmentController.GetSegment<BannerSegment>();
                if (bannerSegment == null)
                {
                    ShowBanner();
                    return;
                }

                if (bannerSegment.IsShowAds(next))
                    ShowBanner();
                else
                    HideBanner();
                


            };*/

            var mediationIntegrationInitInfoPack = StreamingAssetUtils
                // ReSharper disable once RedundantArgumentDefaultValue
                .ReadAllText(MediationIntegrationInitInfo.FileName, false)
                .FromJson<MediationIntegrationInitInfo.MediationIntegrationInitInfoPack>();
            foreach (var medInfo in mediationIntegrationInitInfoPack.allInitInfos)
            {
                var adsIntegration = medInfo.Initialize();
                if (adsIntegration == null) continue;
                var adIntegrationType = adsIntegration.IntegrationType;

                adsIntegration.Initialize(() =>
                {
#if LOG_VERBOSE
                    $"Try to add ads integration {adIntegrationType}".Log();
#endif
                    if (_baseAdsConfig.bannerAdDecision.Contains(adIntegrationType))
                    {
                        UpdateAdsDecider(adsIntegration, AdType.Banner);
                        adsIntegration.OnBannerUpdated += _bannerAdDecider.OnBannerUpdated;
                    }

                    if (_baseAdsConfig.interstitialAdDecision.Contains(adIntegrationType))
                        UpdateAdsDecider(adsIntegration, AdType.Interstitial);
                    if (_baseAdsConfig.rewardedAdDecision.Contains(adIntegrationType))
                        UpdateAdsDecider(adsIntegration, AdType.Rewarded);
                    if (_baseAdsConfig.nativeAdDecision.Contains(adIntegrationType))
                        UpdateAdsDecider(adsIntegration, AdType.NativeAds);
                });
            }


            return;

            void UpdateAdsDecider(BaseAdsIntegration baseAdsIntegration, AdType adType)
            {
                BaseAdDecider adDecider;
                List<AdIntegrationType> adDecision;
                switch (adType)
                {
                    case AdType.Banner:
                        _bannerAdDecider ??= new BannerDecider();
                        adDecider = _bannerAdDecider;
                        adDecision = _baseAdsConfig.bannerAdDecision;
                        break;
                    case AdType.Interstitial:
                        _interstitialAdDecider ??= new InterstitialAdDecider();
                        adDecider = _interstitialAdDecider;
                        adDecision = _baseAdsConfig.interstitialAdDecision;
                        break;
                    case AdType.Rewarded:
                        _rewardedAdDecider ??= new RewardedAdDecider();
                        adDecider = _rewardedAdDecider;
                        adDecision = _baseAdsConfig.rewardedAdDecision;
                        break;
                    case AdType.NativeAds:
                        _nativeAdDecider ??= new NativeAdDecider();
                        adDecider = _nativeAdDecider;
                        adDecision = _baseAdsConfig.nativeAdDecision;
                        break;
                    case AdType.OpenAds:
                        _openAdsDecider ??= new OpenAdsDecider();
                        adDecider = _openAdsDecider;
                        adDecision = _baseAdsConfig.openAdsDecision;
                        break;
                    case AdType.None:
                    default:
                        return;
                }

                if (adDecider.AdIntegrationCount >= adDecision.Count)
                {
#if LOG_VERBOSE
                    "All Ads Integration is ready".Log();
#endif
                    return;
                }

                adDecider.AddAdsIntegration(baseAdsIntegration, adDecision);
            }
        }

        /// <summary>
        /// Show interstitial ads if can show.
        /// </summary>
        /// <param name="onClose"></param>
        /// <param name="placement"></param>
        /// <param name="isShowBreakAds"></param>
        public static void ShowInterstitial(Action onClose, string placement, bool isShowBreakAds)
        {
            if (!IsCanShowInterstitial)
            {
                onClose?.Invoke();
                return;
            }

            _ShowInterstitial(onClose, placement, isShowBreakAds);
        }


        /// <summary>
        /// Show interstitial ads even if it is on cooldown or not loaded
        /// </summary>
        /// <param name="onClose"></param>
        /// <param name="placement"></param>
        /// <param name="isShowBreakAds"></param>
        private static void _ShowInterstitial(Action onClose, string placement, bool isShowBreakAds)
        {
            if (!_interstitialAdDecider.HasAvailableAds)
            {
#if LOG_VERBOSE
                $"No ads integration ready yet".Log();
#endif
                onClose?.Invoke();
                return;
            }

            if (isShowBreakAds)
            {
                BaseAdsConfig.Instance.adBreakPrefab.Show(() =>
                {
                    _interstitialAdDecider.ShowInterstitial(onClose, placement);
                    _interstitialShowTime = DateTime.Now.AddSeconds(InterstitialCooldownAttribute.Value);
                });
            }
            else
            {
                _interstitialAdDecider.ShowInterstitial(onClose, placement);
                _interstitialShowTime = DateTime.Now.AddSeconds(InterstitialCooldownAttribute.Value);
            }
        }

        public static void ShowRewardedVideo(Action onReward, Action onNotReady, Action onClose, string placement)
        {
            switch (_rewardedAdDecider.HasAvailableAds)
            {
                case false when !_interstitialAdDecider.HasAvailableAds:
#if LOG_VERBOSE
                    $"No ads integration ready yet".Log();
#endif
                    onNotReady?.Invoke();
                    return;
                case false:
                    _ShowInterstitial(onReward + onClose, "rw_" + placement, false);
                    return;
                default:
                    _rewardedAdDecider.ShowRewardedVideo(onReward, onNotReady, onClose, placement);
                    break;
            }

            _interstitialShowTime = DateTime.Now.AddSeconds(InterstitialCooldownAttribute.Value);
        }

        public static void ShowBanner()
        {
            if (!_isInitialized)
            {
#if LOG_VERBOSE
                "AdsController is not initialized".Log();
#endif
                return;
            }

            if (IsRemoveAdsAttribute.Value)
            {
                return;
            }

#if LOG_VERBOSE
            "Show Banner".Log();
#endif

            _bannerAdDecider.ShowBanner();
        }

        public static void HideBanner()
        {
            if (!_isInitialized)
            {
#if LOG_VERBOSE
                "AdsController is not initialized".Log();
#endif
                return;
            }

#if LOG_VERBOSE
            "Hide Banner".Log();
#endif
            
            _bannerAdDecider.HideBanner();
        }

        private static void ShowOpenAds(Action onClose, string placement)
        {
            if (!_isInitialized)
            {
#if LOG_VERBOSE
                "AdsController is not initialized".Log();
#endif
                return;
            }

            if (!IsCanShowOpenAds)
            {
                return;
            }

            if (!_openAdsDecider.HasAvailableAds)
            {
#if LOG_VERBOSE
                "No ads integration ready yet".Log();
#endif
                return;
            }

            _openAdsDecider.ShowOpenAds(onClose, placement);
            _openAdsShowTime = DateTime.Now.AddSeconds(30);
        }

#if G_LOG_LOW_LEVEL
        private class DebugAdsController: MonoBehaviour
        {
            [ShowInInspector]
            private BannerDecider debugBannerDecider => _bannerAdDecider;
            
            [ShowInInspector]
            private InterstitialAdDecider debugInterstitialDecider => _interstitialAdDecider;
            
            [ShowInInspector]
            private RewardedAdDecider debugRewardedDecider => _rewardedAdDecider;
            
            [ShowInInspector]
            private OpenAdsDecider debugOpenAdsDecider => _openAdsDecider;
            
            [ShowInInspector]
            private NativeAdDecider debugNativeAdsDecider => _nativeAdDecider;
            
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
            private static void CreateDebug()
            {
                var debug = new GameObject("DebugAdsController");
                debug.AddComponent<DebugAdsController>();
                DontDestroyOnLoad(debug);
            }
        }
#endif
    }
}