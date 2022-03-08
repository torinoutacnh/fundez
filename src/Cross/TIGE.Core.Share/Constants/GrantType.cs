#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> GrantType.cs </Name>
//         <Created> 16/04/2018 8:36:59 PM </Created>
//         <Key> a7adf62c-d5a8-4561-9e82-7f3fb33e87cb </Key>
//     </File>
//     <Summary>
//         GrantType.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TIGE.Core.Share.Constants
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GrantType
    {
        [Display(Name = "Client Credential")]
        ClientCredential,

        [Display(Name = "Authorization Code")]
        AuthorizationCode,

        [Display(Name = "Authorization Code with PKCE")]
        AuthorizationCodePKCE,

        [Display(Name = "Implicit")]
        Implicit,

        [Display(Name = "Resouce Owner Password")]
        ResourceOwner,

        [Display(Name = "Refresh Token")]
        RefreshToken
    }
}