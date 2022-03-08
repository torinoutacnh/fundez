#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> ApplicationModelValidator.cs </Name>
//         <Created> 20/04/2018 11:39:11 AM </Created>
//         <Key> ad42b1d1-6c33-499a-9e7e-8bc30f6f8344 </Key>
//     </File>
//     <Summary>
//         ApplicationModelValidator.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System.Linq;
using FluentValidation;
using TIGE.Core.Share.Models.Application;

namespace TIGE.Core.Share.Validators.Application
{
    public class CreateApplicationModelValidator : AbstractValidator<CreateApplicationModel>
    {
        public CreateApplicationModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }

    public class ApplicationModelValidator : AbstractValidator<ApplicationModel>
    {
        public ApplicationModelValidator()
        {
            Include(new CreateApplicationModelValidator());

            RuleFor(x => x.ListGrantType)
                .Must(x => x?.Any() == true)
                .WithMessage("Grant Types cannot be empty.");

            RuleFor(x => x.JwtExpirationSecond)
                .GreaterThanOrEqualTo(300);
        }
    }
}