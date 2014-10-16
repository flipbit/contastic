using System.Collections.Generic;
using NUnit.Framework;

namespace Contastic
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void TestJoinStrings()
        {
            var input = new[] { "one", "two", "three" };

            var result = input.Join(",");

            Assert.AreEqual("one,two,three", result);
        }

        [Test]
        public void TestJoinStringsWithSomeNullAndEmpty()
        {
            var input = new[] { "one", null, "two", "", "three" };

            var result = input.Join(" ");

            Assert.AreEqual("one two three", result);
        }

        [Test]
        public void TestJoinStringsWithNull()
        {
            IEnumerable<string> input = null;

            var result = input.Join(",");

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void TestJoinStringsWithEmptyList()
        {
            var input = new string[0];

            var result = input.Join(",");

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void TestStripQuotes()
        {
            const string input = @"""Hello World""";

            var result = input.StripQuotes();

            Assert.AreEqual("Hello World", result);
        }

        [Test]
        public void TestStripQuotesWhenEmpty()
        {
            const string input = "";

            var result = input.StripQuotes();

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void TestStripQuotesWhenNull()
        {
            const string input = null;

            var result = input.StripQuotes();

            Assert.AreEqual(string.Empty, result);
        }

    }
}
