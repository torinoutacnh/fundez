#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> AuthorizeModelValidator.cs </Name>
//         <Created> 14/05/2018 4:15:58 PM </Created>
//         <Key> 14240444-c566-46af-a4c6-65f1950b2704 </Key>
//     </File>
//     <Summary>
//         AuthorizeModelValidator.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using TIGE.Core.Models.Authentication;
using FluentValidation;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Validators.Authentication
{
    public class AuthorizeModelValidator : AbstractValidator<AuthorizeModel>
    {
        public AuthorizeModelValidator()
        {
            RuleFor(x => x.Continue).NotEmpty().When(x => x.GrantType == GrantType.AuthorizationCodePKCE || x.GrantType == GrantType.AuthorizationCode || x.GrantType == GrantType.Implicit);

            RuleFor(x => x.CodeChallenge).NotEmpty().When(x => x.GrantType == GrantType.AuthorizationCodePKCE);

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email or UserName must not be empty."); ;
            RuleFor(x => x.Email).MaximumLength(200).WithMessage("Email or UserName must not be less than 200 characters."); ;

            RuleFor(x => x.Password).NotEmpty().Length(6, 100).WithMessage("Password must between 6 and 100 characters");
        }
    }
}