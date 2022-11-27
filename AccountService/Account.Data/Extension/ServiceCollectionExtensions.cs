using System;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Data.Extension
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Register all the configuration of the context.
        /// </summary>
        /// <param name="services">Service Collection</param>
        public static void AddContextConfiguration(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IAccountContext, AccountContext>();
        }
    }
}
