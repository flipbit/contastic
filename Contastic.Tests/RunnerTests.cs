using System;
using Contastic.Samples;
using NUnit.Framework;

namespace Contastic
{
    [TestFixture]
    public class RunnerTests
    {
        private Runner runner;

        [SetUp]
        public void SetUp()
        {
            runner = new Runner();
        }

        [Test]
        public void TestInvokeSingleSwitchCommand()
        {
            runner.Commands.Add(typeof(SingleSwitchCommand));

            var result = runner.Invoke("-s");

            Assert.IsTrue(result.Success);
            Assert.AreEqual(typeof(SingleSwitchCommand), result.CommandType);
        }

        [Test]
        public void TestInvokeSingleSwitchCommandWithMultipleCommands()
        {
            runner.Commands.Add(typeof(SingleSwitchCommand));
            runner.Commands.Add(typeof(MultipleSwitchCommand));

            var result = runner.Invoke("-o -t -3");

            Assert.IsTrue(result.Success);
            Assert.AreEqual(typeof(MultipleSwitchCommand), result.CommandType);
        }
    }
}
