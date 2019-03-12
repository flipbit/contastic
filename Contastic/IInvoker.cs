using System;

namespace Contastic
{
    public interface IInvoker
    {
        object Invoke(Type type);
    }


    public class ActivatorInvoker : IInvoker
    {
        public object Invoke(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
