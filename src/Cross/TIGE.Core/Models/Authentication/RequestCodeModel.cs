#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> RequestCodeModel.cs </Name>
//         <Created> 14/05/2018 2:17:52 PM </Created>
//         <Key> b9add98d-1b51-4a37-9ef0-e3d774add465 </Key>
//     </File>
//     <Summary>
//         RequestCodeModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Models.Authentication
{
    public class RequestCodeModel
    {
        public GrantType GrantType { get; set; } = GrantType.AuthorizationCode;

        /// <summary>
        ///     AuthorizationCode PKCE 
        /// </summary>
        public string CodeChallenge { get; set; }

        /// <summary>
        ///     AuthorizationCode PKCE 
        /// </summary>
        public CodeChallengeMethod CodeChallengeMethod { get; set; } = CodeChallengeMethod.S256;

        public string RedirectUri { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}