using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;
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
        public RegisterUpdateDocumentFlowDtoValidator(IAppUnitOfWork appUnitOfWork, IMapper mapper)
        {
            RuleFor(f => f.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nazwa jest wymagana")
                .MaximumLength(40).WithMessage("Wymagana długość nazwy to 1-40 znaków");

            RuleFor(f => f.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Opis jest wymagany")
                .MinimumLength(5).WithMessage("Wymagana długość nazwy to 5-500 znaków")
                .MaximumLength(500).WithMessage("Wymagana długość nazwy to 5-500 znaków");

            RuleFor(f => f.SelectionMethod)
                .Custom((value, context) => {
                    if (!appUnitOfWork.DocumentFlowRepository.CanChangeSelectionMethod(context.InstanceToValidate.DocumentFlowId, mapper.Map<SelectionMethod>(value)))
                        context.AddFailure("SelectionMethod", "Nie można zmienić metody selekcji, ponieważ istnieją dokumenty, przypisane do flow, oczekujące na sprawdzenie");
                });
        }
    }
}
