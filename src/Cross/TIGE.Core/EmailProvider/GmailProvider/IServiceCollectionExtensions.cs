using System;
using Elect.Core.Attributes;
using Elect.DI;
using Microsoft.Extensions.DependencyInjection;

namespace TIGE.Core.EmailProvider.GmailProvider
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddGmailProvider(this IServiceCollection services, [NotNull]GmailOptions options)
        {
            return services.AddGmailProvider(_ =>
            {
                _.DisplayName = options.DisplayName;
                _.UserName = options.UserName;
                _.Password = options.Password;
            });
        }

        public static IServiceCollection AddGmailProvider(this IServiceCollection services, [NotNull] Action<GmailOptions> configuration)
        {
            services.Configure(configuration);

            services.AddScopedIfNotExist<IEmailProvider, TIGE.Core.EmailProvider.GmailProvider.GmailProvider>();

            return services;
        }




        public static IServiceCollection AddAmazonProvider(this IServiceCollection services, [NotNull] Action<GmailOptions> configuration)
        {
            services.Configure(configuration);
            services.AddScopedIfNotExist<IEmailProvider, TIGE.Core.EmailProvider.GmailProvider.AmazonProvider>();
            return services;
        }

        public static IServiceCollection AddAmazonProvider(this IServiceCollection services, [NotNull] GmailOptions options)
        {
            return services.AddAmazonProvider(_ =>
            {
                _.DisplayName = options.DisplayName;
                _.UserName = options.UserName;
                _.Email = options.Email;
                _.Password = options.Password;
            });
        }
    }
}