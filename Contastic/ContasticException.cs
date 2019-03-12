using System;

namespace Contastic
{
    public class ContasticException : Exception
    {
        public ContasticException(string message) : base(message)
        {
        }
    }
}
