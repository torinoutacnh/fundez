#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> UserModel.cs </Name>
//         <Created> 20/04/2018 5:32:41 PM </Created>
//         <Key> 9270b16c-da24-4442-a03f-bd12b3653bc2 </Key>
//     </File>
//     <Summary>
//         UserModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Share.Models.User
{
    public class ProfileModel
    {
        public string Id { get; set; }
        
        [Display(Name = "Permissions")]
        public List<Permission> ListPermission { get; set; } = new List<Permission>();

        public string Email { get; set; }
        
        [Display(Name = "Email Confirmed At")]
        public DateTimeOffset? EmailConfirmedTime { get; set; }
        
        public string Phone { get; set; }

        [Display(Name = "Phone Confirmed At")]
        public DateTimeOffset? PhoneConfirmedTime { get; set; }


        [Display(Name = "Created At")]
        public DateTimeOffset CreatedTime { get; set; }

        [Display(Name = "Updated At")]
        public DateTimeOffset LastUpdatedTime { get; set; }

        // Editable
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }
}