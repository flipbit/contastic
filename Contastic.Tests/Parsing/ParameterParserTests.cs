using Contastic.Models;
using NUnit.Framework;

namespace Contastic.Parsing
{
    [TestFixture]
    public class ParameterParserTests
    {
        private ParameterParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new ParameterParser();
        }

        [Test]
        public void TestParseSingleVerb()
        {
            var parameters = parser.Parse("one");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual("one", parameters[0].LongName);
            Assert.AreEqual(ParameterType.Verb, parameters[0].Type);
            Assert.AreEqual(string.Empty, parameters[0].Value);
        }

        [Test]
        public void TestParseMultipleVerb()
        {
            var parameters = parser.Parse("one two three");

            Assert.AreEqual(3, parameters.Count);

            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual("one", parameters[0].LongName);
            Assert.AreEqual(ParameterType.Verb, parameters[0].Type);

            Assert.AreEqual(1, parameters[1].Index);
            Assert.AreEqual("two", parameters[1].LongName);
            Assert.AreEqual(ParameterType.Verb, parameters[1].Type);

            Assert.AreEqual(2, parameters[2].Index);
            Assert.AreEqual("three", parameters[2].LongName);
            Assert.AreEqual(ParameterType.Verb, parameters[2].Type);
        }

        [Test]
        public void TestParseSingleVerbWithWhitespace()
        {
            var parameters = parser.Parse("   one   ");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual("one", parameters[0].LongName);
            Assert.AreEqual(ParameterType.Verb, parameters[0].Type);
            Assert.AreEqual(string.Empty, parameters[0].Value);
        }
 
        [Test]
        public void TestParseSingleVerbWithValue()
        {
            var parameters = parser.Parse("one=two");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual("one", parameters[0].LongName);
            Assert.AreEqual(ParameterType.Verb, parameters[0].Type);
            Assert.AreEqual("two", parameters[0].Value);
        }

        [Test]
        public void TestParseSingleVerbWithValueWithColonSeperator()
        {
            var parameters = parser.Parse("one:two");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual("one", parameters[0].LongName);
            Assert.AreEqual(ParameterType.Verb, parameters[0].Type);
            Assert.AreEqual("two", parameters[0].Value);
        }

        [Test]
        public void TestParseSingleVerbWithValueInDoubleQuotes()
        {
            var parameters = parser.Parse(@"one=""two three""");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual("one", parameters[0].LongName);
            Assert.AreEqual(ParameterType.Verb, parameters[0].Type);
            Assert.AreEqual("two three", parameters[0].Value);
        }

        [Test]
        public void TestParseSingleVerbWithValueInSingleQuotes()
        {
            var parameters = parser.Parse(@"one='two three'");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual("one", parameters[0].LongName);
            Assert.AreEqual(ParameterType.Verb, parameters[0].Type);
            Assert.AreEqual("two three", parameters[0].Value);
        }

        [Test]
        public void TestParseSingleSwitch()
        {
            var parameters = parser.Parse("-a");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual('a', parameters[0].ShortName);
            Assert.AreEqual(string.Empty, parameters[0].LongName);
            Assert.AreEqual(ParameterType.Argument, parameters[0].Type);
            Assert.AreEqual(string.Empty, parameters[0].Value);
        }

        [Test]
        public void TestParseMultipleSwitches()
        {
            var parameters = parser.Parse("-abc");

            Assert.AreEqual(3, parameters.Count);

            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual('a', parameters[0].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[0].Type);
            Assert.AreEqual(string.Empty, parameters[0].Value);

            Assert.AreEqual(1, parameters[1].Index);
            Assert.AreEqual('b', parameters[1].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[1].Type);
            Assert.AreEqual(string.Empty, parameters[1].Value);

            Assert.AreEqual(2, parameters[2].Index);
            Assert.AreEqual('c', parameters[2].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[2].Type);
            Assert.AreEqual(string.Empty, parameters[2].Value);
        }

        [Test]
        public void TestParseSingleSwitchWithValue()
        {
            var parameters = parser.Parse("-a=foo");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual('a', parameters[0].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[0].Type);
            Assert.AreEqual("foo", parameters[0].Value);
        }

        [Test]
        public void TestParseSingleSwitchWithValueWithColonSeperator()
        {
            var parameters = parser.Parse("-a:foo");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual('a', parameters[0].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[0].Type);
            Assert.AreEqual("foo", parameters[0].Value);
        }

        [Test]
        public void TestParseSingleSwitchWithValueInDoubleQuotes()
        {
            var parameters = parser.Parse(@"-a="" -foo --bar ''  ""  ");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual('a', parameters[0].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[0].Type);
            Assert.AreEqual(" -foo --bar ''  ", parameters[0].Value);
        }

        [Test]
        public void TestParseSingleSwitchWithValueInSingleQuotes()
        {
            var parameters = parser.Parse(@"-a=' -foo --bar ""  '  ");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual('a', parameters[0].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[0].Type);
            Assert.AreEqual(@" -foo --bar ""  ", parameters[0].Value);
        }

        [Test]
        public void TestParseSingleArgument()
        {
            var parameters = parser.Parse("--arg");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual("arg", parameters[0].LongName);
            Assert.AreEqual(ParameterType.Argument, parameters[0].Type);
            Assert.AreEqual(string.Empty, parameters[0].Value);
        }

        [Test]
        public void TestParseMultipleArguments()
        {
            var parameters = parser.Parse("--arg --arg2:two --arg3='three four'");

            Assert.AreEqual(3, parameters.Count);

            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual("arg", parameters[0].LongName);
            Assert.AreEqual(ParameterType.Argument, parameters[0].Type);
            Assert.AreEqual(string.Empty, parameters[0].Value);

            Assert.AreEqual(1, parameters[1].Index);
            Assert.AreEqual("arg2", parameters[1].LongName);
            Assert.AreEqual(ParameterType.Argument, parameters[1].Type);
            Assert.AreEqual("two", parameters[1].Value);

            Assert.AreEqual(2, parameters[2].Index);
            Assert.AreEqual("arg3", parameters[2].LongName);
            Assert.AreEqual(ParameterType.Argument, parameters[2].Type);
            Assert.AreEqual("three four", parameters[2].Value);
        }

        [Test]
        public void TestParseMultipleTypes()
        {
            var parameters = parser.Parse("verb --arg2:two -abc -d='three four'");

            Assert.AreEqual(6, parameters.Count);

            Assert.AreEqual(0, parameters[0].Index);
            Assert.AreEqual("verb", parameters[0].LongName);
            Assert.AreEqual(ParameterType.Verb, parameters[0].Type);
            Assert.AreEqual(string.Empty, parameters[0].Value);

            Assert.AreEqual(1, parameters[1].Index);
            Assert.AreEqual("arg2", parameters[1].LongName);
            Assert.AreEqual(ParameterType.Argument, parameters[1].Type);
            Assert.AreEqual("two", parameters[1].Value);

            Assert.AreEqual(2, parameters[2].Index);
            Assert.AreEqual('a', parameters[2].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[2].Type);
            Assert.AreEqual(string.Empty, parameters[2].Value);

            Assert.AreEqual(3, parameters[3].Index);
            Assert.AreEqual('b', parameters[3].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[3].Type);
            Assert.AreEqual(string.Empty, parameters[3].Value);

            Assert.AreEqual(4, parameters[4].Index);
            Assert.AreEqual('c', parameters[4].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[4].Type);
            Assert.AreEqual(string.Empty, parameters[4].Value);

            Assert.AreEqual(5, parameters[5].Index);
            Assert.AreEqual('d', parameters[5].ShortName);
            Assert.AreEqual(ParameterType.Argument, parameters[5].Type);
            Assert.AreEqual("three four", parameters[5].Value);
        }

        [Test]
        public void TestParseMultipleTypesWithVerbsAtEnd()
        {
            var parameters = parser.Parse("copy -u:username -p:password source destination");

            Assert.AreEqual(5, parameters.Count);
        }

        [Test]
        public void TestParseQuotedVerbs()
        {
            var parameters = parser.Parse(@"copy 'source arg' ""destination arg""");

            Assert.AreEqual(3, parameters.Count);
            Assert.AreEqual("copy", parameters[0].LongName);
            Assert.AreEqual("source arg", parameters[1].LongName);
            Assert.AreEqual("destination arg", parameters[2].LongName);
        }
    }
}
