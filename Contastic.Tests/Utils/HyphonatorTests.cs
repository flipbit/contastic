using NUnit.Framework;

namespace Contastic.Utils
{
    [TestFixture]
    public class HyphonatorTests
    {
        private StringWriter writer;

        [SetUp]
        public void SetUp()
        {
            writer = new StringWriter(10);
        }

        [Test]
        public void TestWriteLine()
        {
            writer.Write("Hello", 10, 0);

            Assert.AreEqual("Hello", writer.ToString());
        }

        [Test]
        public void TestWriteLineWhenHyponated()
        {
            writer.Write("Hello World", 10, 0);

            Assert.AreEqual("Hello\r\nWorld", writer.ToString());
        }

        [Test]
        public void TestWriteLongLineWhenHyponated()
        {
            writer.Write("HelloWorldLongLine", 10, 0);

            Assert.AreEqual("HelloWorld\r\nLongLine", writer.ToString());
        }

        [Test]
        public void TestWriteLongWordWhenHyponated()
        {
            writer.Write("Hello WorldLongLine", 10, 0);

            Assert.AreEqual("Hello\r\nWorldLongL\r\nine", writer.ToString());
        }
    }
}
