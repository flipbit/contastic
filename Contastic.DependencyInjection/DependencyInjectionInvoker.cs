using System;

namespace Contastic
{
    public class DependencyInjectionInvoker : IInvoker
    {
        private readonly IServiceProvider provider;

        public DependencyInjectionInvoker(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public object Invoke(Type type)
        {
            return provider.GetService(type);
        }
    }
}
