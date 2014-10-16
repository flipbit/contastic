using System.Collections.Generic;
using System.Linq;
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

            commands = commands.OrderBy(c => c.GetType().Name).ToList();

            Assert.AreEqual(3, commands.Count);
            Assert.AreEqual(typeof(TestCommandOne), commands[0].GetType());
            Assert.AreEqual(typeof(TestCommandThree), commands[1].GetType());
            Assert.AreEqual(typeof(TestCommandTwo), commands[2].GetType());
        }

        [Test]
        public void TestFindCommandsWhenNoCommandsFound()
        {
            var commands = finder.Find(typeof(string).Assembly);

            Assert.AreEqual(0, commands.Count);
        }

        [Test]
        public void TestFindCommandsWhenNullAssembly()
        {
            var commands = finder.Find(null);

            Assert.AreEqual(0, commands.Count);
        }

        [Test]
        public void TestCountCommandParameters()
        {
            Assert.AreEqual(2, finder.CountParameters(typeof(TestCommandOne)));
            Assert.AreEqual(3, finder.CountParameters(typeof(TestCommandTwo)));
            Assert.AreEqual(4, finder.CountParameters(typeof(TestCommandThree)));
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
