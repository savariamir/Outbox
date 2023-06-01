using System.Threading.Tasks;

namespace Anshan.Core
{
    /// <summary>
    ///     Represents the unit of work definition
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Begins transaction
        /// </summary>
        Task BeginAsync();

        /// <summary>
        ///     Commits transaction
        /// </summary>
        Task CommitAsync();

        /// <summary>
        ///     Rollback transaction
        /// </summary>
        Task RollbackAsync();
    }
}