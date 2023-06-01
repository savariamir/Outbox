using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Anshan.Persistence.Outbox;
using Dapper;

namespace Anshan.OutboxProcessor.DataStore.Sql
{
    public class SqlOutboxRepository : IOutboxRepository
    {
        private readonly IDbConnection _connection;

        public SqlOutboxRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task UpdateOutboxItemsAsync(IEnumerable<OutboxItem> items)
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

        public async Task<List<OutboxItem>> GetOutboxItemsAsync()
        {
            var items = await _connection.QueryAsync<OutboxItem>("SELECT TOP (100) *  FROM [dbo].[OutboxMessages] where PublishDateTime is NULL");

            return items.ToList();
        }
    }
}