namespace AcceptDocs.Domain.Models
{
    public class DocumentFlowUser
    {
        public int DocumentFlowUserId { get; set; }
        public decimal Value { get; set; }
        public int DocumentFlowId { get; set; }
        public DocumentFlow DocumentFlow { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
