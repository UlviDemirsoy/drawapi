namespace drawapi.Repositories.Abstracts
{
    public interface IUnitOfWork : IDisposable
    {
        IDrawRepository DrawRepository { get; }
        IGroupRepository GroupRepository { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
