namespace AcceptDocs.Domain.Models
{
    public enum AcceptanceRequestStatus
    {
        NotAnswered,
        Accepted,
        Rejected,
    }

    public class AcceptanceRequest
    {
        public int AcceptanceRequestId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public AcceptanceRequestStatus AcceptanceRequestStatus { get; set; } = AcceptanceRequestStatus.NotAnswered;
        public string Feedback { get; set; } = String.Empty;
    }
}
