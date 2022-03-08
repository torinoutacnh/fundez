#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> ConfirmEmailModelValidator.cs </Name>
//         <Created> 21/04/2018 6:07:29 PM </Created>
//         <Key> 1d308e97-47b0-4e99-95f8-a89667f24dd0 </Key>
//     </File>
//     <Summary>
//         ConfirmEmailModelValidator.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using TIGE.Core.Models.Authentication;
using FluentValidation;

namespace TIGE.Core.Validators.Authentication
{
    public class ConfirmEmailModelValidator : AbstractValidator<ConfirmEmailModel>
    {
        public ConfirmEmailModelValidator()
        {
            Include(new SetPasswordModelValidator());
        }
    }
}