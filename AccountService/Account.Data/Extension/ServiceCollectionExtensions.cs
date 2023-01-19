using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Account.Data.Extension
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Register all the configuration of the context.
        /// </summary>
        /// <param name="services">Service Collection</param>
        public static void AddContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<AccountContext>(options => options.UseSqlServer(configuration.GetConnectionString("ChatConnection")));
            services.AddScoped<IAccountContext, AccountContext>();
        }
    }
}
