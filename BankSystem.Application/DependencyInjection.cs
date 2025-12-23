using BankSystem.Application.Interfaces;
using BankSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        RegisterServices(services);
        return services;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IPasswordHasher,  PasswordHasher>();
        services.AddScoped<IHistoryService,  HistoryService>();
    }
}