using Microsoft.Extensions.DependencyInjection;

namespace TIGE.Repository
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddTigeDbContext(this IServiceCollection services)
        {
            TigeDbContextFactoryHelper.Build(services, null);
            
            return services;
        }
    }
}