using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AcceptDocs.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(IAppUnitOfWork appUnitOfWork, IMapper mapper, ILogger<DocumentService> logger)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public Document Create(AddDocumentDto dto)
        {
            var document = _mapper.Map<Document>(dto);
            Document dbDoc = _appUnitOfWork.DocumentRepository.Insert(document);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Utworzono dokument o id: " + dbDoc.DocumentId);
            if (dto.DocumentFlowId != null) {
                var users = _appUnitOfWork.DocumentFlowRepository.GetAttachedUsers((int)dto.DocumentFlowId);
                var selectionMethod = _appUnitOfWork.DocumentFlowRepository.Get((int)dto.DocumentFlowId).SelectionMethod;
                var nextUsers = users.OrderBy(u => u.Value).Select(u => u.User);
                var selectedUser = nextUsers.OrderBy(u => u.AcceptanceRequests.Count(ar => ar.AcceptanceRequestStatus == AcceptanceRequestStatus.NotAnswered)).First();
                if (users.Count > 0) {
                    switch (selectionMethod) {
                        case SelectionMethod.All:
                            foreach (var user in users) {
                                _appUnitOfWork.AcceptanceRequestRepository.Insert(new AcceptanceRequest()
                                {
                                    DocumentId = dbDoc.DocumentId,
                                    UserId = user.UserId,
                                });
                                _logger.LogInformation("Wysłano prośbę o akceptację dokumentu o id: " + dbDoc.DocumentId + " do użytkownika o id: " + user.UserId);
                            }
                            break;
                        case SelectionMethod.AllSequential:
                            _appUnitOfWork.AcceptanceRequestRepository.Insert(new AcceptanceRequest()
                            {
                                DocumentId = dbDoc.DocumentId,
                                UserId = selectedUser.UserId,
                            });
                            _logger.LogInformation("Wysłano prośbę o akceptację dokumentu o id: " + dbDoc.DocumentId + " do użytkownika o id: " + selectedUser.UserId);
                            break;
                        case SelectionMethod.SequentialToFirstQualified:
                            _appUnitOfWork.AcceptanceRequestRepository.Insert(new AcceptanceRequest()
                            {
                                DocumentId = dbDoc.DocumentId,
                                UserId = selectedUser.UserId,
                            });
                            _logger.LogInformation("Wysłano prośbę o akceptację dokumentu o id: " + dbDoc.DocumentId + " do użytkownika o id: " + selectedUser.UserId);
                            break;
                    }
                }
            }
            _appUnitOfWork.Commit();
            return dbDoc;
        }

        public void Delete(int id)
        {
            var document = _appUnitOfWork.DocumentRepository.Get(id);
            _appUnitOfWork.DocumentRepository.Delete(document);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Usunięto dokument o id: " + id);
        }

        public DocumentDto Get(int id)
        {
            var document = _appUnitOfWork.DocumentRepository.Get(id);
            var mappped = _mapper.Map<DocumentDto>(document);
            _logger.LogInformation("Pobrano dokument o id: " + id);
            return mappped;
        }

        public List<DocumentDto> GetAllForUserWithTypeAndFlow(int id)
        {
            var documents = _appUnitOfWork.DocumentRepository.GetAllForUserWithTypeAndFlow(id);
            var mapped = _mapper.Map<List<DocumentDto>>(documents);
            _logger.LogInformation("Pobrano listę dokumentów użytkownika o id: " + id);
            return mapped;
        }

        public DocumentDto GetWithDetails(int id)
        {
            var document = _appUnitOfWork.DocumentRepository.GetWithDetails(id);
            var mapped = _mapper.Map<DocumentDto>(document);
            _logger.LogInformation("Pobrano dokument o id: " + id);
            return mapped;
        }

        public void Update(UpdateDocumentDto dto)
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
            _logger.LogInformation("Zaktualizowano dokument o id: " + document.DocumentId);
            if (dto.DocumentFlowId != null) {
                var toDelete = _appUnitOfWork.AcceptanceRequestRepository.Find(ar => ar.DocumentId == dto.DocumentId).ToList();
                foreach(var accReq in toDelete)
                    _appUnitOfWork.AcceptanceRequestRepository.Delete(accReq);
                var users = _appUnitOfWork.DocumentFlowRepository.GetAttachedUsers((int)dto.DocumentFlowId);
                var selectionMethod = _appUnitOfWork.DocumentFlowRepository.Get((int)dto.DocumentFlowId).SelectionMethod;
                var nextUsers = users.OrderBy(u => u.Value).Select(u => u.User);
                var selectedUser = nextUsers.OrderBy(u => u.AcceptanceRequests.Count(ar => ar.AcceptanceRequestStatus == AcceptanceRequestStatus.NotAnswered)).First();
                if (users.Count > 0) {
                    switch (selectionMethod) {
                        case SelectionMethod.All:
                            foreach (var user in users) {
                                _appUnitOfWork.AcceptanceRequestRepository.Insert(new AcceptanceRequest()
                                {
                                    DocumentId = dto.DocumentId,
                                    UserId = user.UserId,
                                });
                                _logger.LogInformation("Wysłano prośbę o akceptację dokumentu o id: " + dto.DocumentId + " do użytkownika o id: " + user.UserId);
                            }
                            break;
                        case SelectionMethod.AllSequential:
                            _appUnitOfWork.AcceptanceRequestRepository.Insert(new AcceptanceRequest()
                            {
                                DocumentId = dto.DocumentId,
                                UserId = selectedUser.UserId,
                            });
                            _logger.LogInformation("Wysłano prośbę o akceptację dokumentu o id: " + dto.DocumentId + " do użytkownika o id: " + selectedUser.UserId);
                            break;
                        case SelectionMethod.SequentialToFirstQualified:
                            _appUnitOfWork.AcceptanceRequestRepository.Insert(new AcceptanceRequest()
                            {
                                DocumentId = dto.DocumentId,
                                UserId = selectedUser.UserId,
                            });
                            _logger.LogInformation("Wysłano prośbę o akceptację dokumentu o id: " + dto.DocumentId + " do użytkownika o id: " + selectedUser.UserId);
                            break;
                    }
                }
            }
            _appUnitOfWork.Commit();
        }
    }
}
