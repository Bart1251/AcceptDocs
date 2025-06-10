using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AcceptDocs.Application.Services
{
    public class AcceptanceRequestService : IAcceptanceRequestService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AcceptanceRequestService> _logger;

        public AcceptanceRequestService(IAppUnitOfWork appUnitOfWork, IMapper mapper, ILogger<AcceptanceRequestService> logger)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public AcceptanceRequestDto Get(int id)
        {
            var acceptanceRequest = _appUnitOfWork.AcceptanceRequestRepository.GetWithDetails(id);
            var mapped = _mapper.Map<AcceptanceRequestDto>(acceptanceRequest);
            _logger.LogInformation("Pobrano prośbę o akceptację o id: " + id);
            return mapped;
        }

        public List<AcceptanceRequestDto> GetAllForUser(int id)
        {
            var requests = _appUnitOfWork.AcceptanceRequestRepository.GetAllForUser(id);
            var mapped = _mapper.Map<List<AcceptanceRequestDto>>(requests);
            _logger.LogInformation("Pobrano listę próśb o akceptację dla użytkownika o id: " + id);
            return mapped;
        }

        public void GiveFeedback(AddDocumentFeedbackDto dto)
        {
            var acceptanceRequest = _appUnitOfWork.AcceptanceRequestRepository.GetWithDetails(dto.AcceptanceRequestId);
            acceptanceRequest.Feedback = dto.Feedback;
            acceptanceRequest.StatusChangedAt = DateTime.Now;
            acceptanceRequest.AcceptanceRequestStatus = dto.IsDocumentValid ? AcceptanceRequestStatus.Accepted : AcceptanceRequestStatus.Rejected;
            _appUnitOfWork.Commit();
            if (dto.IsDocumentValid)
                _logger.LogInformation("Użytkownik o id: " + acceptanceRequest.UserId + " zaakceptował dokument o id: " + acceptanceRequest.DocumentId);
            else
                _logger.LogInformation("Użytkownik o id: " + acceptanceRequest.UserId + " odrzucił dokument o id: " + acceptanceRequest.DocumentId);

            if (!dto.IsDocumentValid) {
                acceptanceRequest.Document.Status = DocumentStatus.Rejected;
                _logger.LogInformation("Odrzucono dokument o id: " + acceptanceRequest.DocumentId);
            } else {
                var documentFlowUsers = _appUnitOfWork.DocumentFlowRepository.GetAttachedUsers((int)acceptanceRequest.Document.DocumentFlowId);
                var currentValue = documentFlowUsers.FirstOrDefault(dfu => dfu.UserId == acceptanceRequest.UserId)?.Value ?? 0m;
                var nextUsers = documentFlowUsers.OrderBy(u => u.Value).Where(u => u.Value > currentValue).Select(u => u.User);
                switch (acceptanceRequest.Document.DocumentFlow.SelectionMethod) {
                    case SelectionMethod.All:
                        if(_appUnitOfWork.AcceptanceRequestRepository.Find(ar => ar.DocumentId == acceptanceRequest.DocumentId && ar.AcceptanceRequestStatus == AcceptanceRequestStatus.NotAnswered).Count == 0) {
                            acceptanceRequest.Document.Status = DocumentStatus.Approved;
                            _logger.LogInformation("Zaakceptowano dokument o id: " + acceptanceRequest.DocumentId);
                        }
                        break;
                    case SelectionMethod.AllSequential:
                        if (nextUsers.Count() > 0) {
                            var selectedUser = nextUsers.OrderBy(u => u.AcceptanceRequests.Count(ar => ar.AcceptanceRequestStatus == AcceptanceRequestStatus.NotAnswered)).First();
                            _appUnitOfWork.AcceptanceRequestRepository.Insert(new AcceptanceRequest()
                            {
                                DocumentId = acceptanceRequest.DocumentId,
                                UserId = selectedUser.UserId,
                            });
                            _logger.LogInformation("Wysłano prośbę o akceptację dokumentu o id: " + acceptanceRequest.DocumentId + " do użytkownika o id: " + selectedUser.UserId);
                        } else {
                            acceptanceRequest.Document.Status = DocumentStatus.Approved;
                            _logger.LogInformation("Zaakceptowano dokument o id: " + acceptanceRequest.DocumentId);
                        }
                        break;
                    case SelectionMethod.SequentialToFirstQualified:
                        if (nextUsers.Count() > 0 && currentValue < acceptanceRequest.Document.Value) {
                            var selectedUser = nextUsers.OrderBy(u => u.AcceptanceRequests.Count(ar => ar.AcceptanceRequestStatus == AcceptanceRequestStatus.NotAnswered)).First();
                            _appUnitOfWork.AcceptanceRequestRepository.Insert(new AcceptanceRequest()
                            {
                                DocumentId = acceptanceRequest.DocumentId,
                                UserId = selectedUser.UserId,
                            });
                            _logger.LogInformation("Wysłano prośbę o akceptację dokumentu do użytkownika o id: " + selectedUser.UserId);
                        } else {
                            acceptanceRequest.Document.Status = DocumentStatus.Approved;
                            _logger.LogInformation("Zaakceptowano dokument o id: " + acceptanceRequest.DocumentId);
                        }
                        break;
                }
            }
            _appUnitOfWork.Commit();
        }
    }
}
