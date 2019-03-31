namespace Contastic.Utils
{
    public interface IWriter
    {
        void Write(char value);

        IScreen Screen { get; }
    }
}