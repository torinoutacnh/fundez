using System;
using Elect.Core.Attributes;
using Elect.DI;
using Microsoft.Extensions.DependencyInjection;

namespace TIGE.Core.EmailProvider.SendGridProvider
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSendGridEmailProvider(this IServiceCollection services, [NotNull]SendGridOptions options)
        {
            return services.AddSendGridEmailProvider(_ =>
            {
                _.DisplayEmail = options.DisplayEmail;
                _.DisplayName = options.DisplayName;
                _.Key = options.Key;
            });
        }

        public static IServiceCollection AddSendGridEmailProvider(this IServiceCollection services, [NotNull] Action<SendGridOptions> configuration)
        {
            services.Configure(configuration);

            services.AddScopedIfNotExist<IEmailProvider, SendGridEmailProvider>();

            return services;
        }
    }
}