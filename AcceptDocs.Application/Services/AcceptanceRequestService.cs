using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;

namespace AcceptDocs.Application.Services
{
    public class AcceptanceRequestService : IAcceptanceRequestService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;

        public AcceptanceRequestService(IAppUnitOfWork appUnitOfWork, IMapper mapper)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
        }

        public AcceptanceRequestDto Get(int id)
        {
            var acceptanceRequest = _appUnitOfWork.AcceptanceRequestRepository.GetWithDetails(id);
            return _mapper.Map<AcceptanceRequestDto>(acceptanceRequest);
        }

        public List<AcceptanceRequestDto> GetAllForUser(int id)
        {
            var requests = _appUnitOfWork.AcceptanceRequestRepository.GetAllForUser(id);
            return _mapper.Map<List<AcceptanceRequestDto>>(requests);
        }

        public void GiveFeedback(AddDocumentFeedbackDto dto)
        {
            var acceptanceRequest = _appUnitOfWork.AcceptanceRequestRepository.GetWithDetails(dto.AcceptanceRequestId);
            acceptanceRequest.Feedback = dto.Feedback;
            acceptanceRequest.StatusChangedAt = DateTime.Now;
            acceptanceRequest.AcceptanceRequestStatus = dto.IsDocumentValid ? AcceptanceRequestStatus.Accepted : AcceptanceRequestStatus.Rejected;
            _appUnitOfWork.Commit();
            if (!dto.IsDocumentValid) {
                acceptanceRequest.Document.Status = DocumentStatus.Rejected;
            } else {
                var documentFlowUsers = _appUnitOfWork.DocumentFlowRepository.GetAttachedUsers((int)acceptanceRequest.Document.DocumentFlowId);
                var currentValue = documentFlowUsers.FirstOrDefault(dfu => dfu.UserId == acceptanceRequest.UserId)?.Value ?? 0m;
                var nextUsers = documentFlowUsers.OrderBy(u => u.Value).Where(u => u.Value > currentValue).Select(u => u.User);
                switch (acceptanceRequest.Document.DocumentFlow.SelectionMethod) {
                    case SelectionMethod.All:
                        if(_appUnitOfWork.AcceptanceRequestRepository.Find(ar => ar.DocumentId == acceptanceRequest.DocumentId && ar.AcceptanceRequestStatus == AcceptanceRequestStatus.NotAnswered).Count == 0)
                            acceptanceRequest.Document.Status = DocumentStatus.Approved;
                        break;
                    case SelectionMethod.AllSequential:
                        if (nextUsers.Count() > 0) {
                            var selectedUser = nextUsers.OrderBy(u => u.AcceptanceRequests.Count(ar => ar.AcceptanceRequestStatus == AcceptanceRequestStatus.NotAnswered)).First();
                            _appUnitOfWork.AcceptanceRequestRepository.Insert(new AcceptanceRequest()
                            {
                                DocumentId = acceptanceRequest.DocumentId,
                                UserId = selectedUser.UserId,
                            });
                        } else {
                            acceptanceRequest.Document.Status = DocumentStatus.Approved;
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
                        } else {
                            acceptanceRequest.Document.Status = DocumentStatus.Approved;
                        }
                        break;
                }
            }
            _appUnitOfWork.Commit();
        }
    }
}
