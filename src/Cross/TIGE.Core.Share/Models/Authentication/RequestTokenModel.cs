#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> RequestTokenModel.cs </Name>
//         <Created> 22/04/2018 10:57:34 PM </Created>
//         <Key> 7a0fe33e-7123-45ed-a9bb-095849c774e0 </Key>
//     </File>
//     <Summary>
//         RequestTokenModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using TIGE.Core.Share.Constants;

namespace TIGE.Core.Share.Models.Authentication
{
    public class RequestTokenModel
    {
        /// <summary>
        ///     GrantType must be ResourceOwner, Authorization Code (PKCE), RefreshToken 
        /// </summary>
        public GrantType GrantType { get; set; }

        public string State { get; set; }

        /// <summary>
        ///     Authorization Code 
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        ///     Resource Owner Password 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Resource Owner Password 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     AuthorizationCode 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     AuthorizationCode PKCE 
        /// </summary>
        public string CodeVerifier { get; set; }

        /// <summary>
        ///     AuthorizationCode + Implicit 
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        ///     RefreshToken 
        /// </summary>
        public string RefreshToken { get; set; }
    }
}