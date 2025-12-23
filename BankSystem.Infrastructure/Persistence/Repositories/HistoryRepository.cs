using System.Data;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Repositories;
using Dapper;

namespace BankSystem.Infrastructure.Persistence.Repositories;

public class HistoryRepository : BaseRepository, IHistoryRepository
{
    public HistoryRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task<IEnumerable<HistoryEntity>> GetHistoryByCustomerIdAsync(int customerId,
        CancellationToken cancellationToken = default)
    {
        var sql = """
                  select h.*, c.name AS customer_name, os.name AS status_name, ot.name AS type_name
                  from history AS h 
                  join operation_type AS ot on h.operation_type_id = ot.id
                  join operation_status AS os on h.operation_status_id = os.id
                  join customer AS c on h.sender_customer_id = c.id
                  where customer_id = @customerId
                  """;

        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        return await _connection.QueryAsync<HistoryEntity>(command);
    }
}