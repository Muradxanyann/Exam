using System.Data;

namespace BankSystem.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository
{
    protected readonly IDbConnection _connection;

    protected BaseRepository(IDbConnection connection)
    {
        _connection = connection;
    }
}