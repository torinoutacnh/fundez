using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using TIGE.Core.Share.Utils;

namespace TIGE.Binders
{
    public static class TimeSpanModelBinderExtensions
    {
        public static IServiceCollection AddTimeSpanBinder(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                var isProviderAdded =
                    options.ModelBinderProviders.Any(x => x.GetType() == typeof(TimeSpanModelBinderProvider));

                if (isProviderAdded) return;

                options.ModelBinderProviders.Insert(0, new TimeSpanModelBinderProvider());
            });

            return services;
        }
    }

    public class TimeSpanModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.Metadata.UnderlyingOrModelType == typeof(TimeSpan) ? new TimeSpanModelBinder() : null;
        }
    }

    public class TimeSpanModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None) return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            try
            {
                var value = valueProviderResult.FirstValue;

                object model = string.IsNullOrWhiteSpace(value) ? null : value.ToSystemTimeSpan();

                // If model is null and type is not nullable Return a required field error

                if (model == null && !bindingContext.ModelMetadata.IsReferenceOrNullableType)
                {
                    bindingContext.ModelState
                        .TryAddModelError(bindingContext.ModelName,
                            bindingContext.ModelMetadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(
                                valueProviderResult.ToString()));

                    return Task.CompletedTask;
                }

                bindingContext.Result = ModelBindingResult.Success(model);

                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                bindingContext.ModelState
                    .TryAddModelError(bindingContext.ModelName, exception, bindingContext.ModelMetadata);

                return Task.CompletedTask;
            }
        }
    }
}