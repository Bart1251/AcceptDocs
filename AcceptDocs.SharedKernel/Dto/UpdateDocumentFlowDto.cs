namespace AcceptDocs.SharedKernel.Dto
{
    public class UpdateDocumentFlowDto
    {
        public int DocumentFlowId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public SelectionMethodDto SelectionMethod { get; set; }
        public decimal Value { get; set; }
    }
}
