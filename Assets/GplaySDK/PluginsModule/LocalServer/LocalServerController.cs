// Filename: SdkController.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 14:27 15/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.Threading.Tasks;
using GplaySDK.Core.BaseLib;
using GplaySDK.Core.BaseLib.Const;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.LocalServer.Schema;
using GplaySDK.Network;

namespace GplaySDK.LocalServer
{
    public static class LocalServerController
    {
        public static async Task<GetBasicSdkConfigResponse> GetBasicSdkConfig()
        {
            var requestBody = await new GetBasicSdkConfigRequest()
            {
                aliasId = BaseLibConfig.Instance.projectSdkAliasId
            }.ToJsonAsync();
            return await (await NetworkUtils.PostAsync(StringConst.Editor.SdkServer.Url.Core.GetBasicData, requestBody))
                .FromJsonAsync<GetBasicSdkConfigResponse>();
        }

        public static async Task<GetMaxSdkConfigResponse> GetMaxSdkConfig()
        {
            var requestBody = await new GetMaxSdkConfigRequest()
            {
                aliasId = BaseLibConfig.Instance.projectSdkAliasId
            }.ToJsonAsync();
            return await (await NetworkUtils.PostAsync(StringConst.Editor.SdkServer.Url.Core.GetMaxData, requestBody))
                .FromJsonAsync<GetMaxSdkConfigResponse>();
        }
        
        public static async Task<GetAdmobConfigResponse> GetAdmobConfig()
        {
            var requestBody = await new GetAdmobConfigRequest()
            {
                aliasId = BaseLibConfig.Instance.projectSdkAliasId
            }.ToJsonAsync();
            return await (await NetworkUtils.PostAsync(StringConst.Editor.SdkServer.Url.Core.GetAdmobData, requestBody))
                .FromJsonAsync<GetAdmobConfigResponse>();
        }  
        
    }
}