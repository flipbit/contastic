using System;
using System.Collections.Generic;
using Contastic.Models;

namespace Contastic.Parsing
{
    internal class TokenParser
    {
        public IList<Token> Parse(string input)
        {
            var parameters = new List<Token>();

            var enumerator = new TokenEnumerator(input);

            if (enumerator.IsEmpty)
            {
                return parameters;
            }

            var state = TokenParserState.AtStart;
            var parameter = new Token();

            while (enumerator.IsEmpty == false)
            {
                switch (state)
                {
                    case TokenParserState.AtStart:
                        ParseStart(enumerator, ref state);
                        break;

                    case TokenParserState.InPreamble:
                        ParsePreamble(enumerator, ref state);
                        break;

                    case TokenParserState.InVerb:
                        ParseVerb(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case TokenParserState.InSingleQuotedVerb:
                        ParseSingleQuotedVerb(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case TokenParserState.InDoubleQuotedVerb:
                        ParseDoubleQuotedVerb(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case TokenParserState.InValue:
                        ParseValue(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case TokenParserState.InSwitch:
                        ParseSwitch(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case TokenParserState.InArgument:
                        ParseArgument(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case TokenParserState.InDoubleQuotedValue:
                        ParseDoubleQuotedValue(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    case TokenParserState.InSingleQuotedValue:
                        ParseSingleQuotedValue(ref parameters, ref parameter, enumerator, ref state);
                        break;

                    default:
                        throw new ContasticException($"Unknown {nameof(TokenParserState)}: {state}");
                }
            }

            if (parameter.HasName)
            {
                parameters.Add(parameter);
            }

            return parameters;
        }

        private void ParseStart(TokenEnumerator enumerator, ref TokenParserState state)
        {
            var peek = enumerator.Peek();

            if (string.IsNullOrWhiteSpace(peek))
            {
                enumerator.Next();
                return;
            }

            state = TokenParserState.InPreamble;
        }
        
        private void ParsePreamble(TokenEnumerator enumerator, ref TokenParserState state)
        {
            var peek = enumerator.Peek();

            switch (peek)
            {
                case "-":
                    if (enumerator.IsPeek("--"))
                    {
                        state = TokenParserState.InArgument;
                        enumerator.Next(2);
                    }
                    else
                    {
                        state = TokenParserState.InSwitch;
                        enumerator.Next();
                    }
                    break;

                case @"""":
                    state = TokenParserState.InDoubleQuotedVerb;
                    enumerator.Next();
                    break;
                    
                case "'":
                    state = TokenParserState.InSingleQuotedVerb;
                    enumerator.Next();
                    break;

                default:
                    state = TokenParserState.InVerb;
                    break;
            }
        }

        private void ParseVerb(ref List<Token> parameters, ref Token parameter, TokenEnumerator enumerator, ref TokenParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case " ":
                    state = TokenParserState.InPreamble;
                    if (!string.IsNullOrWhiteSpace(parameter.LongName))
                    {
                        parameters.Add(parameter);
                        parameter = new Token
                        {
                            Index = parameters.Count
                        };
                    }
                    break;

                case "=":
                case ":":
                    state = TokenParserState.InValue;
                    break;

                default:
                    parameter.Type = TokenType.Argument;
                    parameter.AppendName(next);
                    break;
            }
        }

        private void ParseValue(ref List<Token> parameters, ref Token parameter, TokenEnumerator enumerator, ref TokenParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case @"""":
                    state = TokenParserState.InDoubleQuotedValue;
                    break;

                case @"'":
                    state = TokenParserState.InSingleQuotedValue;
                    break;

                case " ":
                    state = TokenParserState.InPreamble;
                    if (!string.IsNullOrWhiteSpace(parameter.LongName) ||
                        parameter.ShortName != '\0') 
                    {
                        parameters.Add(parameter);
                        parameter = new Token
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

        private void ParseDoubleQuotedVerb(ref List<Token> parameters, ref Token parameter, TokenEnumerator enumerator, ref TokenParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case @"""":
                    state = TokenParserState.InPreamble;
                    if (parameter.HasName)
                    {
                        parameters.Add(parameter);
                        parameter = new Token
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

        private void ParseSingleQuotedVerb(ref List<Token> parameters, ref Token parameter, TokenEnumerator enumerator, ref TokenParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case "'":
                    state = TokenParserState.InPreamble;
                    if (parameter.HasName)
                    {
                        parameters.Add(parameter);
                        parameter = new Token
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

        private void ParseDoubleQuotedValue(ref List<Token> parameters, ref Token parameter, TokenEnumerator enumerator, ref TokenParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case @"""":
                    state = TokenParserState.InPreamble;
                    if (!string.IsNullOrWhiteSpace(parameter.LongName))
                    {
                        parameters.Add(parameter);
                        parameter = new Token
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

        private void ParseSingleQuotedValue(ref List<Token> parameters, ref Token parameter, TokenEnumerator enumerator, ref TokenParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case @"'":
                    state = TokenParserState.InPreamble;
                    if (!string.IsNullOrWhiteSpace(parameter.LongName))
                    {
                        parameters.Add(parameter);
                        parameter = new Token
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

        private void ParseSwitch(ref List<Token> parameters, ref Token parameter, TokenEnumerator enumerator, ref TokenParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case " ":
                    state = TokenParserState.InPreamble;
                    break;

                case "=":
                case ":":
                    state = TokenParserState.InValue;
                    break;

                default:
                    parameter.ShortName = char.Parse(next);
                    parameter.Type = TokenType.Option;
                    if (enumerator.IsPeek("=", ":") == false)
                    {
                        parameters.Add(parameter);
                        parameter = new Token
                        {
                            Index = parameters.Count
                        };
                    }
                    break;
            }
        }

        private void ParseArgument(ref List<Token> parameters, ref Token parameter, TokenEnumerator enumerator, ref TokenParserState state)
        {
            var next = enumerator.Next();

            switch (next)
            {
                case " ":
                    parameters.Add(parameter);
                    parameter = new Token
                    {
                        Index = parameters.Count
                    };
                    state = TokenParserState.InPreamble;
                    break;

                case "=":
                case ":":
                    state = TokenParserState.InValue;
                    break;

                default:
                    parameter.AppendName(next);
                    parameter.Type = TokenType.Option;
                    break;
            }
        }
    }
}
