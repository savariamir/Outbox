using System.Threading.Tasks;
using Anshan.Core;
using Microsoft.EntityFrameworkCore;

namespace Anshan.EF
{
    public class EfUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;

        public EfUnitOfWork(TContext context)
        {
            _context = context;
        }

        public Task BeginAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _context.Database.CurrentTransaction.CommitAsync();
        }

        public Task RollbackAsync()
        {
            return _context.Database.CurrentTransaction.RollbackAsync();
        }
    }
}