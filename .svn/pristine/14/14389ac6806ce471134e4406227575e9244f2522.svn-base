#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> SetPasswordModelValidator.cs </Name>
//         <Created> 21/04/2018 5:59:29 PM </Created>
//         <Key> e57a8f73-7539-4c28-b371-d911de353cdf </Key>
//     </File>
//     <Summary>
//         SetPasswordModelValidator.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using TIGE.Core.Models.Authentication;
using FluentValidation;

namespace TIGE.Core.Validators.Authentication
{
    public class SetPasswordModelValidator : AbstractValidator<SetPasswordModel>
    {
        public SetPasswordModelValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(6, 100)
                .WithMessage("Password must between 6 and 100 characters");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password)
                .WithMessage("Confirm Password must match with Password");
        }
    }
}