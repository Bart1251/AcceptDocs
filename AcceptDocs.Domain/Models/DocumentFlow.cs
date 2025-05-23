namespace AcceptDocs.Domain.Models
{
    public enum SelectionMethod
    {
        All,
        FirstQualified,
        AllQualified,
    }

    public class DocumentFlow
    {
        public int DocumentFlowId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public SelectionMethod SelectionMethod { get; set; }
        public decimal Value { get; set; }
        public List<Document> Documents { get; set; } = new();
        public List<User> Users { get; set; } = new();
    }
}
