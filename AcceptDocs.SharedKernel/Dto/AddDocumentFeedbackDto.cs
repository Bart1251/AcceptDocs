namespace AcceptDocs.SharedKernel.Dto
{
    public class AddDocumentFeedbackDto
    {
        public int AcceptanceRequestId { get; set; }
        public bool IsDocumentValid { get; set; }
        public string Feedback { get; set; } = String.Empty;
    }
}
