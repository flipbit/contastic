using System.Collections.Generic;
using NUnit.Framework;

namespace Contastic
{
    [TestFixture]
    public class ParameterParserTest
    {
        private ParameterParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new ParameterParser();
        }

        [Test]
        public void TestParseOneParameter()
        {
            var results = parser.Parse("-parameter value");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("parameter", results[0].Name);
            Assert.AreEqual("value", results[0].Value);
        }

        [Test]
        public void TestParseTwoParameters()
        {
            var results = parser.Parse("-first one -second two");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("first", results[0].Name);
            Assert.AreEqual("one", results[0].Value);
            Assert.AreEqual("second", results[1].Name);
            Assert.AreEqual("two", results[1].Value);
        }

        [Test]        
        public void TestParseOneParameterFlag()
        {
            var results = parser.Parse("-first");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("first", results[0].Name);
            Assert.AreEqual("", results[0].Value);
        }

        [Test]
        public void TestParseOneParameterMultipleValues()
        {
            var results = parser.Parse("-first one two three -second one");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("first", results[0].Name);
            Assert.AreEqual("one", results[0].Values[0]);
            Assert.AreEqual("two", results[0].Values[1]);
            Assert.AreEqual("three", results[0].Values[2]);
            Assert.AreEqual("one two three", results[0].Value);
            Assert.AreEqual("second", results[1].Name);
            Assert.AreEqual("one", results[1].Value);
        }

        [Test]
        public void TestParseOneParameterWithQuotes()
        {
            var results = parser.Parse(@"-first ""one two"" three -second one");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("first", results[0].Name);
            Assert.AreEqual("one two", results[0].Values[0]);
            Assert.AreEqual("three", results[0].Values[1]);
            Assert.AreEqual("one two three", results[0].Value);
            Assert.AreEqual("second", results[1].Name);
            Assert.AreEqual("one", results[1].Value);
        }



        [Test]
        public void TestIsASwitchWhenParameter()
        {
            Assert.IsTrue(parser.IsASwitch("-one"));
            Assert.IsTrue(parser.IsASwitch("--two"));
            Assert.IsTrue(parser.IsASwitch("/three"));
        }

        [Test]
        public void TestIsASwitchWhenNotAParameter()
        {
            Assert.IsFalse(parser.IsASwitch("four"));
        }

        [Test]
        public void TestIsASwitchWhenEmpty()
        {
            Assert.IsFalse(parser.IsASwitch(""));
        }

        [Test]
        public void TestIsASwitchWhenNull()
        {
            Assert.IsFalse(parser.IsASwitch(null));
        }

        [Test]
        public void TestIsASwitchWhenParameterCustomOptions()
        {
            var options = new Options();
            options.ParameterSwitches.Add("=");

            Assert.IsTrue(parser.IsASwitch("=one", options));
            Assert.IsFalse(parser.IsASwitch("--two", options));
        }

        [Test]
        public void TestRemoveParameterSwitch()
        {
            Assert.AreEqual("one", parser.RemoveSwitch("-one"));
            Assert.AreEqual("two", parser.RemoveSwitch("--two"));
            Assert.AreEqual("three", parser.RemoveSwitch("/three"));            
        }

        [Test]
        public void TestRemoveParameterSwitchWhenNotAParameter()
        {
            Assert.AreEqual("four", parser.RemoveSwitch("four"));
        }

        [Test]
        public void TestRemoveParameterSwitchWhenEmpty()
        {
            Assert.AreEqual("", parser.RemoveSwitch(""));
        }

        [Test]
        public void TestRemoveParameterSwitchWhenNull()
        {
            Assert.AreEqual("", parser.RemoveSwitch(null));
        }
    }
}
