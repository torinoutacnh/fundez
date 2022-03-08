#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> AuthorizeCodeStorageModel.cs </Name>
//         <Created> 22/04/2018 11:58:09 PM </Created>
//         <Key> 0a818680-6c4c-45ef-a5d2-b5eec1437a02 </Key>
//     </File>
//     <Summary>
//         AuthorizeCodeStorageModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Utils;

namespace TIGE.Core.Models.Authentication
{
    public class AuthorizeCodeStorageModel
    {
        public string ClientId { get; set; }

        public GrantType GrantType { get; set; } = GrantType.AuthorizationCode;

        /// <summary>
        ///     AuthorizationCode PKCE 
        /// </summary>
        public string CodeChallenge { get; set; }

        /// <summary>
        ///     AuthorizationCode PKCE 
        /// </summary>
        public CodeChallengeMethod CodeChallengeMethod { get; set; } = CodeChallengeMethod.S256;

        public string Code { get; set; }

        public string RedirectUri { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTimeOffset CreatedTime { get; set; } = CoreHelper.SystemTimeNow;
    }
}