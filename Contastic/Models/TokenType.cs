namespace Contastic.Models
{
    /// <summary>
    /// The type of command line <see cref="Token"/>.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// A command line argument.
        /// An argument has no prefix (e.g. "-" or "--") and is mapped according to it's index
        /// on the command line.
        /// </summary>
        Argument,
        /// <summary>
        /// A command line option.
        /// An option is mapped via a switch (e.g. "-s" or "--switch").  Options can have
        /// values.
        /// </summary>
        Option
    }
}