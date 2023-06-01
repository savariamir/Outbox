using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anshan.Domain;

/// <summary>
///     The base of all repositories
/// </summary>
public interface IRepository
{
}

public interface IRepository<TAggregateRoot, TKey> : IRepository where TAggregateRoot : IAggregate
{
    Task AddAsync(TAggregateRoot value);

    Task AddAsync(IEnumerable<TAggregateRoot> value);

    Task<TAggregateRoot> GetByIdAsync(TKey id);
    Task<TAggregateRoot> GetByIdAsync(TKey id, int version);

    Task<TDerivedTAggregateRoot> GetByIdAsync<TDerivedTAggregateRoot>(TKey id)
        where TDerivedTAggregateRoot : TAggregateRoot;

    Task<TDerivedTAggregateRoot> GetByIdAsync<TDerivedTAggregateRoot>(TKey id, int version)
        where TDerivedTAggregateRoot : TAggregateRoot;
        
    Task DeleteAsync(TAggregateRoot value);

    Task DeleteAsync(TAggregateRoot value, int version);

    Task DeleteAsync(IEnumerable<TAggregateRoot> values);
        
    Task DeleteHardAsync(TAggregateRoot value);

    Task DeleteHardAsync(TAggregateRoot value, int version);
        
    Task DeleteHardAsync(TKey id);

    Task DeleteHardAsync(TKey id, int version);
        
    Task UpdateAsync(TAggregateRoot value);
        
    Task BulkUpdateAsync(IEnumerable<TAggregateRoot> values);

    Task UpdateAsync(TAggregateRoot value, int version);

    Task<TKey> GetNextIdAsync();

    IAsyncEnumerable<TDerivedTAggregateRoot> StreamAsync<TDerivedTAggregateRoot>()
        where TDerivedTAggregateRoot : TAggregateRoot;

    IAsyncEnumerable<TAggregateRoot> StreamAsync();

    IAsyncEnumerable<TAggregateRoot> StreamAsync(int offset, int size);
    
        
    Task<bool> ExistsAsync(TKey id);

    Task<bool> ExistsAsync(TKey id, int version);

    Task ReSeedTrackerIdAsync(CancellationToken cancellationToken = default);
}