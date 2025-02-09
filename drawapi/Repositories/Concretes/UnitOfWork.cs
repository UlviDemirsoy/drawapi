using drawapi.Data;
using drawapi.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace drawapi.Repositories.Concretes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        public IDrawRepository DrawRepository { get; }
        public IGroupRepository GroupRepository { get; }

        public UnitOfWork(AppDbContext context, IDrawRepository drawRepository, IGroupRepository groupRepository)
        {
            _context = context;
            DrawRepository = drawRepository;
            GroupRepository = groupRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
