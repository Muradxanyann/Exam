using System.Data;
using BankSystem.Domain.Repositories;
using BankSystem.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace BankSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        RegisterServices(services);
        return services;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IDbConnection>(c =>
        {
            var config = c.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("No connection string provided");
            }

            return new NpgsqlConnection(connectionString);
        });
        
        services.AddScoped<ICustomerRepository,  CustomerRepository>();
        services.AddScoped<IHistoryRepository,  HistoryRepository>();
        
       
    } 
}