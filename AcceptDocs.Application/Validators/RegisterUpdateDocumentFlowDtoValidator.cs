using AcceptDocs.SharedKernel.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptDocs.Application.Validators
{
    public class RegisterUpdateDocumentFlowDtoValidator : AbstractValidator<UpdateDocumentFlowDto>
    {
        public RegisterUpdateDocumentFlowDtoValidator()
        {
            RuleFor(f => f.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nazwa jest wymagana")
                .MaximumLength(20).WithMessage("Wymagana długość nazwy to 1-20 znaków");

            RuleFor(f => f.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Opis jest wymagany")
                .MinimumLength(5).WithMessage("Wymagana długość nazwy to 5-500 znaków")
                .MaximumLength(500).WithMessage("Wymagana długość nazwy to 5-500 znaków");
        }
    }
}
