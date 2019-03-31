namespace Contastic.Utils
{
    public static class WriterExtensions
    {
        public static void Write(this IWriter writer, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            foreach (var @char in value)
            {
                writer.Write(@char);
            }
        }
    }
}