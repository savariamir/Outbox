using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Anshan.Persistence.Outbox;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Anshan.OutboxProcessor.DataStore.Sql
{
    public class SqlDataStore : IDataStore
    {
        private readonly ILogger<OutboxWorker> _logger;
        private IDataStoreChangeTracker _changeTracker;
        private readonly IDbConnection _connection;

        public SqlDataStore(ILogger<OutboxWorker> logger, IDbConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        public void SetSubscriber(IDataStoreChangeTracker changeTracker)
        {
            _changeTracker = changeTracker;
        }

        public async Task SubscribeForChanges()
        {
            var items = await GetOutboxItems();
            if (items.Any())
            {
                _logger.LogInformation($"{items.Count} Events found in outbox");
                _changeTracker.ChangeDetected(items);
                await UpdateOutboxItems(items);
            }
        }

        private async Task UpdateOutboxItems(List<OutboxItem> items)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            foreach (var item in items)
            {
                await _connection.ExecuteAsync(
                    "UPDATE OutboxMessages SET PublishDateTime = @PublishDateTime WHERE Id = @OutboxId",
                    new { PublishDateTime = DateTime.UtcNow, OutboxId = item.Id });
            }

            transactionScope.Complete();
        }

        private async Task<List<OutboxItem>> GetOutboxItems()
        {
            var items = await _connection.QueryAsync<OutboxItem>("SELECT TOP (100) *  FROM [dbo].[OutboxMessages] where PublishDateTime is NULL");

            return items.ToList();
        }
    }
}