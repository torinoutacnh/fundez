using Elect.Core.StringUtils;
using Elect.Data.IO.ImageUtils;
using FluentValidation;
using TIGE.Core.Models;
using TIGE.Core.Share.Models.Slot;
using TIGE.Core.Share.Models.Token;
using TIGE.Core.Share.Models.Verify;

namespace TIGE.Core.Validators
{
    public class VerifyModelValidator : AbstractValidator<VerifyModel>
    {
        public VerifyModelValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("You have to input code.");
        }
    } 
    
    public class SubmitTokenModelValidator : AbstractValidator<SubmitTokenModel>
    {
        public SubmitTokenModelValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("You have to input number greater 0.");
        }
    }

    public class SubmitSlotModelValidator : AbstractValidator<SubmitSlotModel>
    {
        public SubmitSlotModelValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("You have to input number greater 0.");
        }
    }
}