using Microsoft.Extensions.DependencyInjection;

namespace Framex.Core
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterFramexServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvc(mvcOptions =>
            {
                mvcOptions.Filters.Add<FramexRequestParseFilter>();
            });
            serviceCollection.AddScoped<IProcessorExecutionContext, ProcessorExecutionContext>();
            serviceCollection.AddTransient<IValidationEngine, ValidationEngine>();
        }
    }
}
