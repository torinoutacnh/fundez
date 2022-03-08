#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> ConfirmEmailModel.cs </Name>
//         <Created> 21/04/2018 5:58:13 PM </Created>
//         <Key> 96614e74-59ee-4505-a5b8-3e1ab8a47ba8 </Key>
//     </File>
//     <Summary>
//         ConfirmEmailModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TIGE.Core.Models.Authentication
{
    public class ConfirmEmailModel : SetPasswordModel
    {
        [Remote("CheckUniqueUserName", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The Email is already exist, please try another.")]
        [Display(Name = "User Name")]
        public string Email { get; set; }
    }
}