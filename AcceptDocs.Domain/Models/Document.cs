using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptDocs.Domain.Models
{
    public enum DocumentStatus
    {
        Created,
        WaitingForApproval,
        Approved,
        Rejected,
    }

    public class Document
    {
        public int DocumentId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string FileName { get; set; } = String.Empty;
        public decimal Value { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DocumentStatus Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? DocumentFlowId { get; set; }
        public DocumentFlow? DocumentFlow { get; set; }
        public List<AcceptanceRequest> AcceptanceRequests { get; set; } = new();
    }
}
