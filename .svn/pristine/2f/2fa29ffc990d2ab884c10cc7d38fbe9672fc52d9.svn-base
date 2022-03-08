#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> UserModelValidator.cs </Name>
//         <Created> 20/04/2018 6:43:31 PM </Created>
//         <Key> e9138277-8823-4808-8eb4-5b93ece461da </Key>
//     </File>
//     <Summary>
//         UserModelValidator.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System;
using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using TIGE.Core.Share.Models.User;

namespace TIGE.Core.Share.Validators.User
{
    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(500);
        }
    }

    public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
    {
        public RegisterUserModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(500);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Refer).NotEmpty().MaximumLength(500).WithMessage("Invalid register link");
            RuleFor(x => x.Code).NotEmpty().MaximumLength(500).WithMessage("Invalid register link");
            RuleFor(x => x.ConfirmPassword).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword)
                .When(x => !string.IsNullOrWhiteSpace(x.Password)).WithMessage("Invalid Confirm Password");
        }
    }

    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(500)
                .When(x => !x.IsApp);

            RuleFor(x => x.Phone)
                .MaximumLength(20);
            
            RuleFor(x => x.BannedRemark)
                .NotEmpty()
                .When(x => x.IsBanned)
                .MaximumLength(500);
        }
    }
}