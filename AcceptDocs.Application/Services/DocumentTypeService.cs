using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AcceptDocs.Application.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentTypeService> _logger;

        public DocumentTypeService(IAppUnitOfWork appUnitOfWork, IMapper mapper, ILogger<DocumentTypeService> logger)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public bool CanDelete(int id)
        {
            return _appUnitOfWork.DocumentTypeRepository.CanDelete(id);
        }

        public int Create(DocumentTypeDto dto)
        {
            var documentType = _mapper.Map<DocumentType>(dto);
            DocumentType dbType = _appUnitOfWork.DocumentTypeRepository.Insert(documentType);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Utworzono typ dokumentu o id: " + dbType.DocumentTypeId);
            return dbType.DocumentTypeId;
        }

        public void Delete(int id)
        {
            var documentType = _appUnitOfWork.DocumentTypeRepository.Get(id);
            _appUnitOfWork.DocumentTypeRepository.Delete(documentType);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Usunięto typ dokumentu o id: " + id);
        }

        public DocumentTypeDto Get(int id)
        {
            var documentType = _appUnitOfWork.DocumentTypeRepository.Get(id);
            var mappped = _mapper.Map<DocumentTypeDto>(documentType);
            _logger.LogInformation("Pobrano typ dokumentu o id: " + id);
            return mappped;
        }

        public List<DocumentTypeDto> GetAll()
        {
            var documentTypes = _appUnitOfWork.DocumentTypeRepository.GetAll();
            var mappped = _mapper.Map<List<DocumentTypeDto>>(documentTypes);
            _logger.LogInformation("Pobrano listę wszystkich typów dokumentów");
            return mappped;
        }

        public void Update(DocumentTypeDto dto)
        {
            var documentType = _appUnitOfWork.DocumentTypeRepository.Get(dto.DocumentTypeId);
            documentType.Name = dto.Name;
            _appUnitOfWork.Commit();
            _logger.LogInformation("Zaktualizowano typ dokumentu o id: " + dto.DocumentTypeId);
        }
    }
}
