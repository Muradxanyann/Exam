using System.Data;
using BankSystem.Domain.DTOs;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Repositories;
using Dapper;

namespace BankSystem.Infrastructure.Persistence.Repositories;

public class CustomerRepository : BaseRepository, ICustomerRepository
{
    public CustomerRepository(IDbConnection connection) : base(connection) {}

    public async Task<IEnumerable<CustomerEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = """SELECT * FROM customer""";
        var command = new CommandDefinition(sql, cancellationToken);
        var customers =  await _connection.QueryAsync<CustomerDbRow>(command);
        return customers.Select(u => CustomerEntity.Restore(
            u.Id,
            u.Name,
            u.Email,
            u.PhoneNumber,
            u.PasswordHash,
            u.PasswordSalt,
            u.Balance,
            u.IsActive
        ));
    }

    public async Task<CustomerEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var sql = """Select * FROM customer WHERE id = @Id""";
        var command = new CommandDefinition(sql, new { Id = id },  cancellationToken : cancellationToken);
        var customer =  await _connection.QuerySingleOrDefaultAsync<CustomerDbRow>(command);
        if (customer == null)
            return null;
        
        return CustomerEntity.Restore(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.PhoneNumber,
            customer.PasswordHash,
            customer.PasswordSalt,
            customer.Balance,
            customer.IsActive
        );
    }

    public async Task<int> InsertAsync(CustomerEntity entity, CancellationToken cancellationToken = default)
    {
        var sql = """
                   INSERT INTO customer (name, email, phone_number, password_hash, password_salt, balance)
                    VALUES (@Name , @Email, @PhoneNumber, @PasswordHash, @PasswordSalt, @Balance)
                    RETURNING id
                  """;
        var command = new CommandDefinition(sql, entity, cancellationToken : cancellationToken);
        return await _connection.ExecuteScalarAsync<int>(command);
    }

    public async Task<bool> UpdateAsync(CustomerEntity entity, CancellationToken ct)
    {
        const string sql = """
                               UPDATE customer
                               SET
                                   name = @Name,
                                   phone_number = @PhoneNumber
                               WHERE id = @Id
                           """;
        var affected = await _connection.ExecuteAsync(
            new CommandDefinition(sql, entity, cancellationToken: ct)
        );
        return affected > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var sql = """UPDATE customer SET is_active = false WHERE id = @Id""";
        var command = new CommandDefinition(sql, new { Id = id },  cancellationToken : cancellationToken);
        return await _connection.ExecuteAsync(command) > 0;
    }

    public async Task<CustomerEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        const string sql = """SELECT * FROM customer WHERE email = @Email""";
        var command = new CommandDefinition(sql, new { Email = email },  cancellationToken : cancellationToken);
        return await _connection.QueryFirstOrDefaultAsync<CustomerEntity?>(command);
    }

    public async Task<bool> UpdateBalanceAsync(int customerId, decimal balance, CancellationToken cancellationToken = default)
    {
        var sql = """UPDATE customer SET balance = @Balance WHERE id = @Id""";
        var command = new CommandDefinition(sql, new { Balance = balance, Id = customerId },  cancellationToken : cancellationToken);
        return await _connection.ExecuteAsync(command) > 0;

    }
}