using AcceptDocs.SharedKernel.Dto;
using FluentValidation;

namespace AcceptDocs.Application.Validators
{
    public class RegisterPositionLevelDtoValidator : AbstractValidator<PositionLevelDto>
    {
        public RegisterPositionLevelDtoValidator()
        {
            RuleFor(pl => pl.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nazwa jest wymagana")
                .MinimumLength(2).WithMessage("Wymagana sługość nazwy to 2-30 znaków")
                .MaximumLength(30).WithMessage("Wymagana sługość nazwy to 2-30 znaków");
        }
    }
}
