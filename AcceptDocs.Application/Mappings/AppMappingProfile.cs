
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
            CreateMap<DocumentFlow, DocumentFlowDto>().
                ForMember(m => m.Users, c => c.MapFrom(s => s.DocumentFlowUsers));
            CreateMap<DocumentFlowDto, DocumentFlow>();
            CreateMap<DocumentFlow, AddDocumentFlowDto>().ReverseMap();
            CreateMap<DocumentFlow, UpdateDocumentFlowDto>().ReverseMap();
            CreateMap<DocumentFlowUser, AttachUserDto>().ReverseMap();
            CreateMap<DocumentFlowUser, DocumentFlowUserDto>()
                .ForMember(m => m.FirstName, c => c.MapFrom(s => s.User.FirstName))
                .ForMember(m => m.LastName, c => c.MapFrom(s => s.User.LastName))
                .ForMember(m => m.Position, c => c.MapFrom(s => s.User.Position));
            CreateMap<Document, DocumentDto>().ReverseMap();
            CreateMap<AcceptanceRequest, AcceptanceRequestDto>().ReverseMap();
            CreateMap<AcceptanceRequestStatus, AcceptanceRequestStatusDto>().ReverseMap();
            CreateMap<DocumentStatus, DocumentStatusDto>().ReverseMap();
            CreateMap<SelectionMethod, SelectionMethodDto>().ReverseMap();
        }
    }
}
