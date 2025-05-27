namespace AcceptDocs.SharedKernel.Dto
{
    public class AddDocumentFlowDto
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public SelectionMethodDto SelectionMethod { get; set; }
    }
}
