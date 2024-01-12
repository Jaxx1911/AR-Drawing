using System;
using GoogleMobileAds.Ump.Api;
using GplaySDK.Core.BaseLib.Utils;
using UnityEngine.Events;

namespace GplaySDK.AdmobIntegration
{
    internal static class Cmp
    {
        private static Action<bool> _onConsentValidated;

        public static void ShowConsentForm(Action<bool> onConsentValidated)
        {
            _onConsentValidated = val => ThreadUtils.RunOnMainThread(() => onConsentValidated(val));

            // Set tag for under age of consent.
            // Here false means users are not under age of consent.
            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
            };

            // Check the current consent information status.
            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }

        private static void OnConsentInfoUpdated(FormError consentError)
        {
            if (consentError != null)
            {
                // Handle the error.
#if LOG_ERROR
                ("OnConsentInfoUpdated Error: " + consentError.Message).LogError();
#endif
                _onConsentValidated?.Invoke(false);
                return;
            }

            // If the error is null, the consent information state was updated.
            // You are now ready to check if a form is available.
            if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
            {
                ConsentForm.Load((form, error) =>
                {
                    if (error != null)
                    {
#if LOG_ERROR
                        ("OnConsentInfoUpdated| ConsentForm load error: " + error.Message).LogError();
#endif
                        _onConsentValidated?.Invoke(false);
                        return;
                    }

                    form.Show(formError =>
                    {
                        if (formError != null)
                        {
#if LOG_ERROR
                            ("OnConsentInfoUpdated| ConsentForm load error: " + formError.Message).LogError();
#endif
                            _onConsentValidated?.Invoke(false);
                            return;
                        }

                        _onConsentValidated?.Invoke(true);
                    });
                });
            }
            else
            {
#if LOG_VERBOSE
                ("OnConsentInfoUpdated ConsentStatus: " + ConsentInformation.ConsentStatus).Log();
#endif
                _onConsentValidated?.Invoke(true);
            }
        }

        /// <summary>
        /// Check if it's necessary to show the Privacy Options Form
        /// </summary>
        public static bool RequirePrivacyOptionsForm => ConsentInformation.PrivacyOptionsRequirementStatus ==
                                                        PrivacyOptionsRequirementStatus.Required;

        /// <summary>
        /// Show Privacy Options form if required
        /// </summary>
        public static void ShowPrivacyOptionsForm(UnityAction actionError)
        {
            ConsentForm.ShowPrivacyOptionsForm(formError =>
            {
                if (formError != null)
                {
#if LOG_ERROR
                    ("ShowPrivacyOptionsForm Error: " + formError.Message).LogError();
#endif
                    actionError?.Invoke();
                }
            });
        }
    }
}