#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> UserSignInTrackingModel.cs </Name>
//         <Created> 01/05/2018 9:25:06 AM </Created>
//         <Key> 8889358c-c07c-4863-a54f-e1ee1934c4d6 </Key>
//     </File>
//     <Summary>
//         UserSignInTrackingModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using TIGE.Core.Utils;
using System;
using System.Collections.Generic;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Utils;

namespace TIGE.Core.Models.Authentication
{
    public class UserSignInTrackingModel
    {
        public string Id { get; set; }
        
        public string Email { get; set; }
        public bool Enable2FA { get; set; }
        public int AuthyId { get; set; }

        public string Phone { get; set; }
        public List<Permission> Permissions { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; } = CoreHelper.SystemTimeNow;

        public bool IsLoggedIn { get; set; } = true;
    }
}