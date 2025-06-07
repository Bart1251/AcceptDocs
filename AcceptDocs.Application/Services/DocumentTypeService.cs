using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;

namespace AcceptDocs.Application.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;

        public DocumentTypeService(IAppUnitOfWork appUnitOfWork, IMapper mapper)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
        }

        public int Create(DocumentTypeDto dto)
        {
            var documentType = _mapper.Map<DocumentType>(dto);
            DocumentType dbType = _appUnitOfWork.DocumentTypeRepository.Insert(documentType);
            _appUnitOfWork.Commit();
            return dbType.DocumentTypeId;
        }

        public void Delete(int id)
        {
            var documentType = _appUnitOfWork.DocumentTypeRepository.Get(id);
            _appUnitOfWork.DocumentTypeRepository.Delete(documentType);
            _appUnitOfWork.Commit();
        }

        public DocumentTypeDto Get(int id)
        {
            var documentType = _appUnitOfWork.DocumentTypeRepository.Get(id);
            return _mapper.Map<DocumentTypeDto>(documentType);
        }

        public List<DocumentTypeDto> GetAll()
        {
            var documentTypes = _appUnitOfWork.DocumentTypeRepository.GetAll();
            return _mapper.Map<List<DocumentTypeDto>>(documentTypes);
        }

        public void Update(DocumentTypeDto dto)
        {
            var documentType = _appUnitOfWork.DocumentTypeRepository.Get(dto.DocumentTypeId);
            documentType.Name = dto.Name;
            _appUnitOfWork.Commit();
        }
    }
}
