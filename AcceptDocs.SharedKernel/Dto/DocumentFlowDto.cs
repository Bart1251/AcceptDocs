using System.ComponentModel;
using System.Reflection.Metadata;

namespace AcceptDocs.SharedKernel.Dto
{
    public enum SelectionMethodDto
    {
        [Description("Wszyscy")]
        All,
        [Description("Pierwszy spełniający kryteria")]
        FirstQualified,
        [Description("Wszyscy spełniający kryteria")]
        AllQualified,
    }

    public class DocumentFlowDto
    {
        public int DocumentFlowId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public SelectionMethodDto SelectionMethod { get; set; }
        public decimal Value { get; set; }
        public List<DocumentDto> Documents { get; set; } = new();
        public List<UserDto> Users { get; set; } = new();
    }
}
