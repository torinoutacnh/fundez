#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> SetPasswordModel.cs </Name>
//         <Created> 21/04/2018 5:57:36 PM </Created>
//         <Key> 5d13de7d-0d80-4d23-b7d1-5ce9f670f90d </Key>
//     </File>
//     <Summary>
//         SetPasswordModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System.ComponentModel.DataAnnotations;

namespace TIGE.Core.Models.Authentication
{
    public class SetPasswordModel
    {
        public string Token { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}