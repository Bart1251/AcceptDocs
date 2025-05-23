
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;

namespace AcceptDocs.Application.Mappings
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, AddUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<PositionLevel, PositionLevelDto>().ReverseMap();
            CreateMap<DocumentType, DocumentTypeDto>().ReverseMap();
            CreateMap<DocumentFlow, DocumentFlowDto>().ReverseMap();
            CreateMap<DocumentFlow, AddDocumentFlowDto>().ReverseMap();
            CreateMap<DocumentFlow, UpdateDocumentFlowDto>().ReverseMap();
            CreateMap<Document, DocumentDto>().ReverseMap();
            CreateMap<AcceptanceRequest, AcceptanceRequestDto>().ReverseMap();
            CreateMap<AcceptanceRequestStatus, AcceptanceRequestStatusDto>().ReverseMap();
            CreateMap<DocumentStatus, DocumentStatusDto>().ReverseMap();
            CreateMap<SelectionMethod, SelectionMethodDto>().ReverseMap();
        }
    }
}
