using AcceptDocs.SharedKernel.Dto;
using FluentValidation;

namespace AcceptDocs.BlazorClient.Validators
{
    public class RegisterAddDocumentFeedbackDtoValidator : AbstractValidator<AddDocumentFeedbackDto>
    {
        public RegisterAddDocumentFeedbackDtoValidator()
        {
            RuleFor(df => df.Feedback)
                .MaximumLength(500).WithMessage("Maksymalna długośc informacji zwrotnej to 500 znaków");
        }
    }
}
