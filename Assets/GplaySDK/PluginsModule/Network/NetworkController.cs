// Filename: NetworkController.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 10:43 14/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Net.Http;
using System.Threading.Tasks;
using GplaySDK.Core.BaseLib.Utils;

namespace GplaySDK.Network
{
    internal static class NetworkController
    {
        private static HttpClient _baseClient;

        private static bool _isInit;

        private static void Init()
        {
            if (_isInit) return;
            _baseClient = new HttpClient();
            _baseClient.DefaultRequestHeaders.Add("User-Agent", "GplaySDK");
            _baseClient.Timeout = new System.TimeSpan(0, 0, 10);
            _isInit = true;
        }

        public static async Task<string> Post(string url, string body)
        {
            Init();
            var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
#if LOG_VERBOSE
            $"Post to url: {url}".Log();
#endif
            var response = await _baseClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
#if LOG_ERROR
                $"Post failed with status code: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}"
                    .LogError();
#endif
                return null;
            }
#if LOG_VERBOSE
            "Post success".Log();
#endif
            return await response.Content.ReadAsStringAsync();
        }
    }
}