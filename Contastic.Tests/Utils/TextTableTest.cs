using NUnit.Framework;

namespace Contastic.Utils
{
    [TestFixture]
    public class TextTableTest
    {
        private TextTable table;

        [SetUp]
        public void SetUp()
        {
            table = new TextTable
            {
                NewLine = "\n", 
                ColumnSeparator = "  "
            };
        }

        [Test]
        public void TestOneRowOneColumnTable()
        {
            table.AddRow("one");

            var text = table.ToString();

            Assert.AreEqual("one\n", text);
        }

        [Test]
        public void TestOneRowTwoColumnTable()
        {
            table.AddRow("one", "two");

            var text = table.ToString();

            Assert.AreEqual("one  two\n", text);
        }

        [Test]
        public void TestTwoRowTwoColumnTable()
        {
            table.AddRow("one", "two");
            table.AddRow("three", "four");

            var text = table.ToString();

            Assert.AreEqual("  one   two\nthree  four\n", text);
        }

        [Test]
        public void TestTwoRowTwoColumnTableWithAlignment()
        {
            table.Align(Align.Left, Align.Left);
            table.AddRow("one", "two");
            table.AddRow("three", "four");

            var text = table.ToString();

            Assert.AreEqual("one    two \nthree  four\n", text);
        }
    }
}
