using System.ComponentModel;

namespace AcceptDocs.SharedKernel.Dto
{
    public enum AcceptanceRequestStatusDto
    {
        [Description("Nie odpowiedziano")]
        NotAnswered,
        [Description("Zaakceptowano")]
        Accepted,
        [Description("Odrzucono")]
        Rejested,
    }

    public class AcceptanceRequestDto
    {
        public int AcceptanceRequestId { get; set; }
        public UserDto User { get; set; }
        public DocumentDto Document { get; set; }
        public AcceptanceRequestStatusDto AcceptanceRequestStatus { get; set; }
        public string Feedback { get; set; } = String.Empty;
    }
}
