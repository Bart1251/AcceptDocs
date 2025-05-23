namespace AcceptDocs.Domain.Contracts
{
    public interface IAppUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }
        public IPositionLevelRepository PositionLevelRepository { get; }
        //repositories

        void Commit();
    }
}
