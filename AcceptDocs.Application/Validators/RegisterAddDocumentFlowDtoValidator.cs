using AcceptDocs.Domain.Contracts;
using AcceptDocs.SharedKernel.Dto;
using FluentValidation;

namespace AcceptDocs.Application.Validators
{
    public class RegisterAddDocumentFlowDtoValidator : AbstractValidator<AddDocumentFlowDto>
    {
        public RegisterAddDocumentFlowDtoValidator(IAppUnitOfWork appUnitOfWork)
        {
            RuleFor(f => f.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nazwa jest wymagana")
                .MaximumLength(40).WithMessage("Wymagana długość nazwy to 1-40 znaków")
                .Custom((value, context) => {
                    if (appUnitOfWork.DocumentFlowRepository.IsNameUsed(value))
                        context.AddFailure("Name", "Istnieje już przepływ z taką nazwą");
                });

            RuleFor(f => f.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Opis jest wymagany")
                .MinimumLength(5).WithMessage("Wymagana długość opisu to 5-500 znaków")
                .MaximumLength(500).WithMessage("Wymagana długość opisu to 5-500 znaków");
        }
    }
}
