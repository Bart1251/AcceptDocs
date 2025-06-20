﻿using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;

namespace AcceptDocs.SharedKernel.Dto
{
    public class AddDocumentDto
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public int DocumentTypeId { get; set; }
        public string FileName { get; set; } = String.Empty;
        [JsonIgnore]
        public IBrowserFile File { get; set; }
        public decimal Value { get; set; }
        public DocumentStatusDto Status { get; set; }
        public int UserId { get; set; }
        public int? DocumentFlowId { get; set; }
    }
}
