using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;

namespace AcceptDocs.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;

        public DocumentService(IAppUnitOfWork appUnitOfWork, IMapper mapper)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
        }

        public int Create(AddDocumentDto dto)
        {
            var document = _mapper.Map<Document>(dto);
            int id = _appUnitOfWork.DocumentRepository.Insert(document);
            _appUnitOfWork.Commit();
            return id;
        }

        public void Delete(int id)
        {
            var document = _appUnitOfWork.DocumentRepository.Get(id);
            _appUnitOfWork.DocumentRepository.Delete(document);
            _appUnitOfWork.Commit();
        }

        public DocumentDto Get(int id)
        {
            var document = _appUnitOfWork.DocumentRepository.Get(id);
            return _mapper.Map<DocumentDto>(document);
        }

        public List<DocumentDto> GetAllForUserWithTypeAndFlow(int id)
        {
            var documents = _appUnitOfWork.DocumentRepository.GetAllForUserWithTypeAndFlow(id);
            return _mapper.Map<List<DocumentDto>>(documents);
        }

        public DocumentDto GetWithTypeFlowAndUser(int id)
        {
            var document = _appUnitOfWork.DocumentRepository.GetWithTypeFlowAndUser(id);
            return _mapper.Map<DocumentDto>(document);
        }

        public void Update(AddDocumentDto dto)
        {
            var document = _appUnitOfWork.DocumentRepository.Get(dto.DocumentId);
            document.Name = dto.Name;
            document.Description = dto.Description; 
            document.DocumentTypeId = dto.DocumentTypeId;
            document.FileName = dto.FileName;
            document.Value = dto.Value;
            document.Status = _mapper.Map<DocumentStatus>(dto.Status);
            document.DocumentFlowId = dto.DocumentFlowId;
            _appUnitOfWork.Commit();
        }
    }
}
