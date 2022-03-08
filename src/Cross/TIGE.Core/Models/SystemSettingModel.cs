#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> SystemSettingModel.cs </Name>
//         <Created> 21/04/2018 5:32:14 PM </Created>
//         <Key> 6bfdfa36-1e93-4463-9120-81cd77730f24 </Key>
//     </File>
//     <Summary>
//         SystemSettingModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System;
using System.Reflection;

namespace TIGE.Core.Models
{
    public class SystemSettingModel
    {
        public static SystemSettingModel Instance { get; set; }

        public string ApplicationName { get; set; } = Assembly.GetEntryAssembly().GetName().Name;
        public string NotifyEmail { get; set; }

        public Guid EncryptKey { get; set; }

        public int AuthorizeCodeStorageSeconds { get; set; } = 600;

        public string Domain { get; set; }
        public bool IsEnforceHttps { get; set; }
        public string ReferFormatLink { get; set; }
        public string ExchangeRateEndpoint { get; set; }
        public string GenerateQRCodeEndpoint { get; set; }
        public string AuthyAppId { get; set; }
        public string AuthyAppSecretKey { get; set; }
        public string AuthyAppName { get; set; }
        public string AuthyAppWebSite { get; set; }
        public string AuthyAppApiKey { get; set; }
        public string AuthyAppAccessKey { get; set; }
        public string AuthyApiSigningKey { get; set; }

    }
}