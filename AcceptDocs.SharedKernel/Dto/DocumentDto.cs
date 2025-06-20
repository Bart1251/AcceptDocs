﻿using System.ComponentModel;
using System.Xml.Linq;

namespace AcceptDocs.SharedKernel.Dto
{
    public enum DocumentStatusDto
    {
        [Description("Utworzony")]
        Created,
        [Description("W procesie akceptacji")]
        WaitingForApproval,
        [Description("Zaakceptowany")]
        Approved,
        [Description("Odrzucony")]
        Rejected,
    }

    public class DocumentDto
    {
        public int DocumentId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public DocumentTypeDto DocumentType { get; set; }
        public string FileName { get; set; } = String.Empty;
        public decimal Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DocumentStatusDto Status { get; set; }
        public UserDto User { get; set; }
        public DocumentFlowDto DocumentFlow { get; set; }
        public List<AcceptanceRequestDto> AcceptanceRequests { get; set; } = new();
    }
}
