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
    }
}
