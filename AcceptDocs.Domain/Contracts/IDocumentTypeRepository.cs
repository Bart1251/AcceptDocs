﻿using AcceptDocs.Domain.Models;

namespace AcceptDocs.Domain.Contracts
{
    public interface IDocumentTypeRepository : IRepository<DocumentType>
    {
        bool CanDelete(int id);
    }
}
