using System.Collections.Generic;
using Contastic.Commands;
using NUnit.Framework;

namespace Contastic
{
    [TestFixture]
    public class CommandFinderTest
    {
        private CommandFinder finder;

        [SetUp]
        public void SetUp()
        {
            finder = new CommandFinder();
        }

        [Test]
        public void TestFindCommands()
        {
            var commands = finder.Find(GetType().Assembly);

            Assert.AreEqual(2, commands.Count);
        }

        [Test]
        public void TestCountCommandParameters()
        {
            Assert.AreEqual(2, finder.CountParameters(typeof(TestCommandOne)));
            Assert.AreEqual(3, finder.CountParameters(typeof(TestCommandTwo)));
        }

        [Test]
        public void TestSortCommandsByParameterCount()
        {
            var commands = new List<ICommand>
            {
                new TestCommandOne(),
                new TestCommandTwo()
            };

            var results = finder.SortCommands(commands);

            Assert.AreEqual(typeof(TestCommandTwo), results[0].GetType());
            Assert.AreEqual(typeof(TestCommandOne), results[1].GetType());
        }
    }
}
