#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> GrantTypeHelper.cs </Name>
//         <Created> 01/05/2018 10:08:38 AM </Created>
//         <Key> 930393d7-0605-4f67-a031-0c5dfea27b9d </Key>
//     </File>
//     <Summary>
//         GrantTypeHelper.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System.Collections.Generic;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;

namespace TIGE.Core.Utils
{
    public static class GrantTypeHelper
    {
        public static bool IsAllowAuthorizeFlow(GrantType grantType)
        {
            List<GrantType> allowTypes = new List<GrantType>
            {
                GrantType.Implicit,
                GrantType.AuthorizationCode,
                GrantType.AuthorizationCodePKCE
            };

            return allowTypes.Contains(grantType);
        }

        public static bool IsAllowGenerateToken(GrantType grantType)
        {
            List<GrantType> allowTypes = new List<GrantType>
            {
                GrantType.RefreshToken,
                GrantType.AuthorizationCode,
                GrantType.AuthorizationCodePKCE,
                GrantType.ResourceOwner,
                GrantType.ClientCredential
            };

            return allowTypes.Contains(grantType);
        }

        public static void CheckAllowAuthorizeFlow(GrantType grantType)
        {
            if (!IsAllowAuthorizeFlow(grantType))
            {
                throw new CoreException(nameof(ErrorCode.GrantTypeInValid), ErrorCode.GrantTypeInValid);
            }
        }

        public static void CheckAllowGenerateToken(GrantType grantType)
        {
            if (!IsAllowGenerateToken(grantType))
            {
                throw new CoreException(nameof(ErrorCode.GrantTypeInValid), ErrorCode.GrantTypeInValid);
            }
        }
    }
}