namespace Contastic.Models
{
    internal enum ParameterParserState
    {
        AtStart,
        InPreamble,
        InVerb,
        InValue,
        InSwitch,
        InDoubleQuotedValue,
        InSingleQuotedValue,
        InArgument,
        InDoubleQuotedVerb,
        InSingleQuotedVerb
    }
}
