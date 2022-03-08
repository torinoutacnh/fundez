#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> AuthorizeModel.cs </Name>
//         <Created> 22/04/2018 11:09:50 PM </Created>
//         <Key> 873dcce2-1aa8-4d41-9f96-639fb39925e6 </Key>
//     </File>
//     <Summary>
//         AuthorizeModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System;
using System.ComponentModel.DataAnnotations;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Models.Authentication
{
    public class AuthorizeModel
    {
        /// <summary>
        ///     Redirect URI 
        /// </summary>
        public string Continue { get; set; }

        public GrantType GrantType { get; set; } = GrantType.ResourceOwner;

        public string State { get; set; }

        /// <summary>
        ///     AuthorizationCode PKCE 
        /// </summary>
        public string CodeChallenge { get; set; }

        /// <summary>
        ///     AuthorizationCode PKCE 
        /// </summary>
        public CodeChallengeMethod CodeChallengeMethod { get; set; } = CodeChallengeMethod.S256;

        /// <summary>
        ///     Hint - pre-enter User Name 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Hint - pre-enter Password 
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}