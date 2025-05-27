using System.ComponentModel;

namespace AcceptDocs.SharedKernel.Dto
{
    public enum SelectionMethodDto
    {
        [Description("Wszyscy")]
        All,
        [Description("Wszyscy sekwencyjnie")]
        AllSequential,
        [Description("Sekwencyjnie do spełniającego kryterium")]
        SequentialToFirstQualified,
    }

    public class DocumentFlowDto
    {
        public int DocumentFlowId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public SelectionMethodDto SelectionMethod { get; set; }
        public List<DocumentDto> Documents { get; set; } = new();
        public List<DocumentFlowUserDto> Users { get; set; } = new();
    }
}
