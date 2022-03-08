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
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileModel>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
        }
    }

}