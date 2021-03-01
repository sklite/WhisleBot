using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wbcl.DAL.Context;

namespace Wbcl.DAL
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabaseConnector(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IUsersContext, UsersContext>(
                ServiceLifetime.Transient);
            return services.AddSingleton<IHistoryLogger, HistoryLogger>();
        }
    }
}
