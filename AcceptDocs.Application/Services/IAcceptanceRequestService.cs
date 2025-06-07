using AcceptDocs.SharedKernel.Dto;

namespace AcceptDocs.Application.Services
{
    public interface IAcceptanceRequestService
    {
        List<AcceptanceRequestDto> GetAllForUser(int id);
        void GiveFeedback(AddDocumentFeedbackDto dto);
        AcceptanceRequestDto Get(int id);
    }
}
