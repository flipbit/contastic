using System;
using System.Collections.Generic;
using Contastic.Models;

namespace Contastic.Parsing
{
    public class ParameterParser
    {
        public IList<Parameter> Parse(string input)
        {
            var parameters = new List<Parameter>();

            var enumerator = new ParameterEnumerator(input);

            if (enumerator.IsEmpty)
            {
                return parameters;
            }

            var state = ParameterParserState.AtStart;
            var parameter = new Parameter();

            while (enumerator.IsEmpty == false)
            {
                switch (state)
                {
                    case ParameterParserState.AtStart:
                        ParseStart(enumerator, ref state);
                        break;

                    case ParameterParserState.InPreamble:
                        ParsePreamble(enumerator, ref state);
                        break;

                    case ParameterParserState.InVerb:
                        ParseVerb(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case ParameterParserState.InSingleQuotedVerb:
                        ParseSingleQuotedVerb(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case ParameterParserState.InDoubleQuotedVerb:
                        ParseDoubleQuotedVerb(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case ParameterParserState.InValue:
                        ParseValue(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case ParameterParserState.InSwitch:
                        ParseSwitch(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case ParameterParserState.InArgument:
                        ParseArgument(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case ParameterParserState.InDoubleQuotedValue:
                        ParseDoubleQuotedValue(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case ParameterParserState.InSingleQuotedValue:
                        ParseSingleQuotedValue(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    default:
                        throw new ContasticException($"Unknown {nameof(ParameterParserState)}: {state}");
                }
            }

            if (parameter.HasName)
            {
                parameters.Add(parameter);
            }

            return parameters;
        }

        private void ParseStart(ParameterEnumerator enumerator, ref ParameterParserState state)
        {
            var peek = enumerator.Peek();

            if (string.IsNullOrWhiteSpace(peek))
            {
                enumerator.Next();
                return;
            }

            state = ParameterParserState.InPreamble;
        }
        
        private void ParsePreamble(ParameterEnumerator enumerator, ref ParameterParserState state)
        {
            var peek = enumerator.Peek();

            switch (peek)
            {
                case "-":
                    if (enumerator.IsPeek("--"))
                    {
                        state = ParameterParserState.InArgument;
                        enumerator.Next(2);
                    }
                    else
                    {
                        state = ParameterParserState.InSwitch;
                        enumerator.Next();
                    }
                    break;

                case @"""":
                    state = ParameterParserState.InDoubleQuotedVerb;
                    enumerator.Next();
                    break;
                    
                case "'":
                    state = ParameterParserState.InSingleQuotedVerb;
                    enumerator.Next();
                    break;

                default:
                    state = ParameterParserState.InVerb;
                    break;
            }
        }

        private void ParseVerb(ref List<Parameter> parameters, ref Parameter parameter, ParameterEnumerator enumerator, ref ParameterParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case " ":
                    state = ParameterParserState.InPreamble;
                    if (!string.IsNullOrWhiteSpace(parameter.LongName))
                    {
                        parameters.Add(parameter);
                        parameter = new Parameter
                        {
                            Index = parameters.Count
                        };
                    }
                    break;

                case "=":
                case ":":
                    state = ParameterParserState.InValue;
                    break;

                default:
                    parameter.Type = ParameterType.Verb;
                    parameter.AppendName(next);
                    break;
            }
        }

        private void ParseValue(ref List<Parameter> parameters, ref Parameter parameter, ParameterEnumerator enumerator, ref ParameterParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case @"""":
                    state = ParameterParserState.InDoubleQuotedValue;
                    break;

                case @"'":
                    state = ParameterParserState.InSingleQuotedValue;
                    break;

                case " ":
                    state = ParameterParserState.InPreamble;
                    if (!string.IsNullOrWhiteSpace(parameter.LongName) ||
                        parameter.ShortName != '\0') 
                    {
                        parameters.Add(parameter);
                        parameter = new Parameter
                        {
                            Index = parameters.Count
                        };
                    }

                    break;

                default:
                    parameter.AppendValue(next);
                    break;
            }
        }

        private void ParseDoubleQuotedVerb(ref List<Parameter> parameters, ref Parameter parameter, ParameterEnumerator enumerator, ref ParameterParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case @"""":
                    state = ParameterParserState.InPreamble;
                    if (parameter.HasName)
                    {
                        parameters.Add(parameter);
                        parameter = new Parameter
                        {
                            Index = parameters.Count
                        };
                    }
                    break;

                default:
                    parameter.AppendName(next);
                    break;
            }
        }

        private void ParseSingleQuotedVerb(ref List<Parameter> parameters, ref Parameter parameter, ParameterEnumerator enumerator, ref ParameterParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case "'":
                    state = ParameterParserState.InPreamble;
                    if (parameter.HasName)
                    {
                        parameters.Add(parameter);
                        parameter = new Parameter
                        {
                            Index = parameters.Count
                        };
                    }
                    break;

                default:
                    parameter.AppendName(next);
                    break;
            }
        }

        private void ParseDoubleQuotedValue(ref List<Parameter> parameters, ref Parameter parameter, ParameterEnumerator enumerator, ref ParameterParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case @"""":
                    state = ParameterParserState.InPreamble;
                    if (!string.IsNullOrWhiteSpace(parameter.LongName))
                    {
                        parameters.Add(parameter);
                        parameter = new Parameter
                        {
                            Index = parameters.Count
                        };
                    }
                    break;

                default:
                    parameter.AppendValue(next);
                    break;
            }
        }

        private void ParseSingleQuotedValue(ref List<Parameter> parameters, ref Parameter parameter, ParameterEnumerator enumerator, ref ParameterParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case @"'":
                    state = ParameterParserState.InPreamble;
                    if (!string.IsNullOrWhiteSpace(parameter.LongName))
                    {
                        parameters.Add(parameter);
                        parameter = new Parameter
                        {
                            Index = parameters.Count
                        };
                    }
                    break;

                default:
                    parameter.AppendValue(next);
                    break;
            }
        }

        private void ParseSwitch(ref List<Parameter> parameters, ref Parameter parameter, ParameterEnumerator enumerator, ref ParameterParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case " ":
                    state = ParameterParserState.InPreamble;
                    break;

                case "=":
                case ":":
                    state = ParameterParserState.InValue;
                    break;

                default:
                    parameter.ShortName = char.Parse(next);
                    parameter.Type = ParameterType.Argument;
                    if (enumerator.IsPeek("=", ":") == false)
                    {
                        parameters.Add(parameter);
                        parameter = new Parameter
                        {
                            Index = parameters.Count
                        };
                    }
                    break;
            }
        }

        private void ParseArgument(ref List<Parameter> parameters, ref Parameter parameter, ParameterEnumerator enumerator, ref ParameterParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case " ":
                    parameters.Add(parameter);
                    parameter = new Parameter
                    {
                        Index = parameters.Count
                    };
                    state = ParameterParserState.InPreamble;
                    break;

                case "=":
                case ":":
                    state = ParameterParserState.InValue;
                    break;

                default:
                    parameter.AppendName(next);
                    parameter.Type = ParameterType.Argument;
                    break;
            }
        }
    }
}
