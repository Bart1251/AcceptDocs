namespace AcceptDocs.Domain.Contracts
{
    public interface IAppUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }
        public IPositionLevelRepository PositionLevelRepository { get; }
        public IDocumentFlowRepository DocumentFlowRepository { get; }
        public IDocumentTypeRepository DocumentTypeRepository { get; }
        //repositories

        void Commit();
    }
}
