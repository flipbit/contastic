using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Contastic
{
    public static class ServiceCollectionExtensions
    {
        public static void AddContastic(this IServiceCollection services)
        {
            services.AddTransient<IInteractiveRunner, InteractiveRunner>(provider =>
            {
                var invoker = new DependencyInjectionInvoker(provider);
                
                var commands = provider
                    .GetServices<ICommand>()
                    .Select(c => c.GetType());

                return new InteractiveRunner(commands, invoker);
            });
        }

        public static void AddTransientCommand<T>(this IServiceCollection services) where T : class, ICommand
        {
            services.AddTransient<T>();
            services.AddTransient<ICommand>(provider => provider.GetService<T>());
        }
    }
}
