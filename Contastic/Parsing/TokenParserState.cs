namespace Contastic.Parsing
{
    internal enum TokenParserState
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
