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

using FluentValidation;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.ProtectionFee;

namespace TIGE.Core.Share.Validators
{
    public class CreateProtectionFeeModelValidator : AbstractValidator<CreateProtectionFeeModel>
    {
        public CreateProtectionFeeModelValidator()
        {
            RuleFor(x => x.From).GreaterThanOrEqualTo(0).WithMessage("From must greater than 0 number");
            RuleFor(x => x.Fee).GreaterThanOrEqualTo(0).WithMessage("Fee must greater than 0 number");
            RuleFor(x => x.To).GreaterThan(x=>x.From).WithMessage("To must greater than From number");
        }
    }


    public class ProtectionFeeModelValidator : AbstractValidator<DetailProtectionFeeModel>
    {
        public ProtectionFeeModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.From).GreaterThanOrEqualTo(0).WithMessage("From must greater than 0 number");
            RuleFor(x => x.Fee).GreaterThanOrEqualTo(0).WithMessage("Fee must greater than 0 number");
            RuleFor(x => x.To).GreaterThan(x => x.From).WithMessage("To must greater than From number");
        }
    }
}