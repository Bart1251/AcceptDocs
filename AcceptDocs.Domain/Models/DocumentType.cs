﻿namespace AcceptDocs.Domain.Models
{
    public class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public string Name { get; set; } = String.Empty;
        public List<Document> Documents { get; set; } = new();
    }
}
