using Contastic.Parsing;
using Contastic.Samples;
using NUnit.Framework;

namespace Contastic.Binding
{
    [TestFixture]
    public class ParameterBinderTests
    {
        private ParameterBinder binder;
        private ParameterParser parser;

        [SetUp]
        public void SetUp()
        {
            binder = new ParameterBinder();
            parser = new ParameterParser();
        }

        [Test]
        public void TestBindSingleVerbCommandWhenBound()
        {
            var parameters = parser.Parse("single-class-verb");

            var result = binder.Bind<SingleVerbCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(1, result.BoundVerbs.Count);
            Assert.AreEqual("single-class-verb", result.BoundVerbs[0].Name);
            Assert.AreEqual(0, result.BoundVerbs[0].Index);
        }

        [Test]
        public void TestBindSingleVerbCommandWhenNotBound()
        {
            var parameters = parser.Parse("single-class-verb-miss");

            var result = binder.Bind<SingleVerbCommand>(parameters);

            Assert.IsFalse(result.Bound);
            Assert.IsNull(result.Value);

            Assert.AreEqual(1, result.UnboundVerbs.Count);
            Assert.AreEqual("single-class-verb", result.UnboundVerbs[0].Name);

            Assert.AreEqual(1, result.UnknownVerbs.Count);
            Assert.AreEqual("single-class-verb-miss", result.UnknownVerbs[0].Name);
        }

        [Test]
        public void TestBindSingleArgumentCommandWhenBoundWithLongName()
        {
            var parameters = parser.Parse("--single-argument");

            var result = binder.Bind<SingleArgumentCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(1, result.BoundArguments.Count);
            Assert.AreEqual("single-argument", result.BoundArguments[0].LongName);
            Assert.AreEqual(0, result.BoundArguments[0].Index);
        }

        [Test]
        public void TestBindSingleArgumentCommandWhenBoundWithLongNameAndValue()
        {
            var parameters = parser.Parse("--single-argument=foo");

            var result = binder.Bind<SingleArgumentCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(1, result.BoundArguments.Count);
            Assert.AreEqual("single-argument", result.BoundArguments[0].LongName);
            Assert.AreEqual(0, result.BoundArguments[0].Index);

            Assert.AreEqual("foo", result.Value.Argument);
        }

        [Test]
        public void TestBindSingleArgumentCommandWhenBoundWithLongNameAndVerbValue()
        {
            var parameters = parser.Parse("--single-argument foo");

            var result = binder.Bind<SingleArgumentCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(0, result.BoundVerbs.Count);
            Assert.AreEqual(0, result.UnknownVerbs.Count);

            Assert.AreEqual(1, result.BoundArguments.Count);
            Assert.AreEqual("single-argument", result.BoundArguments[0].LongName);
            Assert.AreEqual("foo", result.BoundArguments[0].Value);
            Assert.AreEqual(0, result.BoundArguments[0].Index);

            Assert.AreEqual("foo", result.Value.Argument);
        }

        [Test]
        public void TestBindSingleArgumentCommandWhenBoundWithLongNameMissing()
        {
            var parameters = parser.Parse("--single-argument-miss");

            var result = binder.Bind<SingleArgumentCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(1, result.UnboundArguments.Count);
            Assert.AreEqual("single-argument", result.UnboundArguments[0].LongName);
            Assert.AreEqual(0, result.UnboundArguments[0].Index);

            Assert.AreEqual(1, result.UnknownArguments.Count);
            Assert.AreEqual("single-argument-miss", result.UnknownArguments[0].LongName);
        }

        [Test]
        public void TestBindSingleArgumentCommandWhenNotBoundWithLongNameMissing()
        {
            var parameters = parser.Parse("--single-argument-miss");

            var result = binder.Bind<SingleArgumentRequiredCommand>(parameters);

            Assert.IsFalse(result.Bound);
            Assert.IsNull(result.Value);

            Assert.AreEqual(1, result.UnboundArguments.Count);
            Assert.AreEqual("single-argument", result.UnboundArguments[0].LongName);
            Assert.AreEqual(0, result.UnboundArguments[0].Index);
        }

        [Test]
        public void TestBindSingleArgumentCommandWhenBoundWithShortName()
        {
            var parameters = parser.Parse("-s");

            var result = binder.Bind<SingleArgumentCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(1, result.BoundArguments.Count);
            Assert.AreEqual('s', result.BoundArguments[0].ShortName);
            Assert.AreEqual(0, result.BoundArguments[0].Index);
        }

        [Test]
        public void TestBindSingleArgumentCommandWhenBoundWithShortNameWithValue()
        {
            var parameters = parser.Parse("-s=foo");

            var result = binder.Bind<SingleArgumentCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(1, result.BoundArguments.Count);
            Assert.AreEqual('s', result.BoundArguments[0].ShortName);
            Assert.AreEqual(0, result.BoundArguments[0].Index);

            Assert.AreEqual("foo", result.Value.Argument);
        }

        [Test]
        public void TestBindSingleArgumentCommandWhenBoundWithShortNameWithVerbValue()
        {
            var parameters = parser.Parse("-s foo");

            var result = binder.Bind<SingleArgumentCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(0, result.BoundVerbs.Count);
            Assert.AreEqual(0, result.UnknownVerbs.Count);

            Assert.AreEqual(1, result.BoundArguments.Count);
            Assert.AreEqual('s', result.BoundArguments[0].ShortName);
            Assert.AreEqual("foo", result.BoundArguments[0].Value);
            Assert.AreEqual(0, result.BoundArguments[0].Index);

            Assert.AreEqual("foo", result.Value.Argument);
        }

        [Test]
        public void TestBindSingleArgumentCommandWhenBoundWithShortNameWhenArgumentMissing()
        {
            var parameters = parser.Parse("-d");

            var result = binder.Bind<SingleArgumentCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(1, result.UnboundArguments.Count);
            Assert.AreEqual('s', result.UnboundArguments[0].ShortName);
            Assert.AreEqual(0, result.UnboundArguments[0].Index);

            Assert.AreEqual(1, result.UnknownArguments.Count);
            Assert.AreEqual('d', result.UnknownArguments[0].ShortName);
        }

        [Test]
        public void TestBindSingleArgumentCommandWhenNotBoundWithShortName()
        {
            var parameters = parser.Parse("-d");

            var result = binder.Bind<SingleArgumentRequiredCommand>(parameters);

            Assert.IsFalse(result.Bound);
            Assert.IsNull(result.Value);

            Assert.AreEqual(1, result.UnboundArguments.Count);
            Assert.AreEqual('s', result.UnboundArguments[0].ShortName);
            Assert.AreEqual(0, result.UnboundArguments[0].Index);
        }

        [Test]
        public void TestBindSingleSwitchCommandWhenBoundWithLongNameAndSet()
        {
            var parameters = parser.Parse("--single-switch");

            var result = binder.Bind<SingleSwitchCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(1, result.BoundSwitches.Count);
            Assert.AreEqual("single-switch", result.BoundSwitches[0].LongName);
            Assert.AreEqual(0, result.BoundSwitches[0].Index);

            Assert.IsTrue(result.Value.Flag);
        }

        [Test]
        public void TestBindSingleSwitchCommandWhenBoundWithShortNameAndSet()
        {
            var parameters = parser.Parse("-s");

            var result = binder.Bind<SingleSwitchCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(1, result.BoundSwitches.Count);
            Assert.AreEqual('s', result.BoundSwitches[0].ShortName);
            Assert.AreEqual(0, result.BoundSwitches[0].Index);

            Assert.IsTrue(result.Value.Flag);
        }

        [Test]
        public void TestBindSingleSwitchCommandWhenBoundWithShortNameAndNotSet()
        {
            var parameters = parser.Parse("-foo");

            var result = binder.Bind<SingleSwitchCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(1, result.UnboundSwitches.Count);
            Assert.AreEqual('s', result.UnboundSwitches[0].ShortName);
            Assert.AreEqual(0, result.UnboundSwitches[0].Index);

            Assert.IsFalse(result.Value.Flag);
        }

        [Test]
        public void TestBindSingleSwitchCommandWhenNotBound()
        {
            var parameters = parser.Parse("-foo");

            var result = binder.Bind<SingleSwitchRequiredCommand>(parameters);

            Assert.IsFalse(result.Bound);
            Assert.IsNull(result.Value);

            Assert.AreEqual(1, result.UnboundSwitches.Count);
            Assert.AreEqual('s', result.UnboundSwitches[0].ShortName);
            Assert.AreEqual(0, result.UnboundSwitches[0].Index);
        }

        [Test]
        public void TestBindMultipleArguments()
        {
            var parameters = parser.Parse("--one:1 --two=2 --three:3");

            var result = binder.Bind<MultipleArgumentCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(3, result.TotalBound);
            Assert.AreEqual(0, result.TotalUnbound);

            Assert.AreEqual("1", result.Value.One);
            Assert.AreEqual("2", result.Value.Two);
            Assert.AreEqual("3", result.Value.Three);
        }

        [Test]
        public void TestBindMultipleArgumentsWhenSomeArgumentsMissing()
        {
            var parameters = parser.Parse("--one:1 --two=2");

            var result = binder.Bind<MultipleArgumentCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(2, result.TotalBound);
            Assert.AreEqual(1, result.TotalUnbound);

            Assert.AreEqual("1", result.Value.One);
            Assert.AreEqual("2", result.Value.Two);
            Assert.AreEqual(null, result.Value.Three);
        }

        [Test]
        public void TestBindMultipleArgumentsWhenRequiredArgumentMissing()
        {
            var parameters = parser.Parse("-3:1 --two=2");

            var result = binder.Bind<MultipleArgumentCommand>(parameters);

            Assert.IsFalse(result.Bound);
            Assert.IsNull(result.Value);

            Assert.AreEqual(2, result.TotalBound);
            Assert.AreEqual(1, result.TotalUnbound);

            Assert.AreEqual(1, result.UnboundArguments.Count);
            Assert.AreEqual("one", result.UnboundArguments[0].LongName);
            Assert.AreEqual('o', result.UnboundArguments[0].ShortName);
            Assert.AreEqual(0, result.UnboundArguments[0].Index);
        }

        [Test]
        public void TestBindMultipleSwitches()
        {
            var parameters = parser.Parse("--one --two --three");

            var result = binder.Bind<MultipleSwitchCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(3, result.TotalBound);
            Assert.AreEqual(0, result.TotalUnbound);

            Assert.AreEqual(true, result.Value.One);
            Assert.AreEqual(true, result.Value.Two);
            Assert.AreEqual(true, result.Value.Three);
        }

        [Test]
        public void TestBindMultipleSwitchesWhenSomeSwitchesMissing()
        {
            var parameters = parser.Parse("--one --two");

            var result = binder.Bind<MultipleSwitchCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(2, result.TotalBound);
            Assert.AreEqual(1, result.TotalUnbound);

            Assert.AreEqual(true, result.Value.One);
            Assert.AreEqual(true, result.Value.Two);
            Assert.AreEqual(false, result.Value.Three);
        }

        [Test]
        public void TestBindMultipleSwitchesWhenRequiredSwitchMissing()
        {
            var parameters = parser.Parse("-3 --two");

            var result = binder.Bind<MultipleSwitchCommand>(parameters);

            Assert.IsFalse(result.Bound);
            Assert.IsNull(result.Value);

            Assert.AreEqual(2, result.TotalBound);
            Assert.AreEqual(1, result.TotalUnbound);

            Assert.AreEqual(1, result.UnboundSwitches.Count);
            Assert.AreEqual("one", result.UnboundSwitches[0].LongName);
            Assert.AreEqual('o', result.UnboundSwitches[0].ShortName);
            Assert.AreEqual(0, result.UnboundSwitches[0].Index);
        }

        [Test]
        public void TestBindUnnamedParameters()
        {
            var parameters = parser.Parse("copy source destination");

            var result = binder.Bind<CopyCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(3, result.TotalBound);
            Assert.AreEqual(2, result.TotalUnbound);

            Assert.AreEqual(2, result.BoundArguments.Count);
            Assert.AreEqual("source", result.BoundArguments[0].Value);
            Assert.AreEqual("destination", result.BoundArguments[1].Value);

            Assert.AreEqual("source", result.Value.Source);
            Assert.AreEqual("destination", result.Value.Destination);
        }

        [Test]
        public void TestBindUnnamedParametersWithArgumentsBefore()
        {
            var parameters = parser.Parse("copy -u:username -p:password source destination");

            var result = binder.Bind<CopyCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(5, result.TotalBound);
            Assert.AreEqual(0, result.TotalUnbound);

            Assert.AreEqual(4, result.BoundArguments.Count);
            Assert.AreEqual("source", result.BoundArguments[2].Value);
            Assert.AreEqual("destination", result.BoundArguments[3].Value);

            Assert.AreEqual("source", result.Value.Source);
            Assert.AreEqual("destination", result.Value.Destination);
            Assert.AreEqual("username", result.Value.UserName);
            Assert.AreEqual("password", result.Value.Password);
        }

        [Test]
        public void TestBindUnnamedParametersWithArgumentsAfter()
        {
            var parameters = parser.Parse("copy source destination -u:username -p:password");

            var result = binder.Bind<CopyCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(5, result.TotalBound);
            Assert.AreEqual(0, result.TotalUnbound);

            Assert.AreEqual(4, result.BoundArguments.Count);
            Assert.AreEqual("source", result.BoundArguments[2].Value);
            Assert.AreEqual("destination", result.BoundArguments[3].Value);

            Assert.AreEqual("source", result.Value.Source);
            Assert.AreEqual("destination", result.Value.Destination);
            Assert.AreEqual("username", result.Value.UserName);
            Assert.AreEqual("password", result.Value.Password);
        }

        [Test]
        public void TestBindUnnamedParametersWithArgumentsMixedIn()
        {
            var parameters = parser.Parse(@"copy ""source path"" -u:username 'destination location' -p:password");

            var result = binder.Bind<CopyCommand>(parameters);

            Assert.IsTrue(result.Bound);
            Assert.IsNotNull(result.Value);

            Assert.AreEqual(5, result.TotalBound);
            Assert.AreEqual(0, result.TotalUnbound);

            Assert.AreEqual(4, result.BoundArguments.Count);
            Assert.AreEqual("source path", result.BoundArguments[2].Value);
            Assert.AreEqual("destination location", result.BoundArguments[3].Value);

            Assert.AreEqual("source path", result.Value.Source);
            Assert.AreEqual("destination location", result.Value.Destination);
            Assert.AreEqual("username", result.Value.UserName);
            Assert.AreEqual("password", result.Value.Password);
        }

        [Test]
        public void TestCanBindUnnamedParametersWithArgumentsMixedIn()
        {
            var parameters = parser.Parse(@"copy ""source path"" -u:username 'destination location' -p:password");

            var result = binder.CanBind<CopyCommand>(parameters);

            Assert.IsTrue(result.Bound);

            Assert.AreEqual(5, result.TotalBound);
            Assert.AreEqual(0, result.TotalUnbound);

            Assert.AreEqual(4, result.BoundArguments.Count);
            Assert.AreEqual("source path", result.BoundArguments[2].Value);
            Assert.AreEqual("destination location", result.BoundArguments[3].Value);
        }

        [Test]
        public void TestCanBindVerbDoesNotBindToEmptyShortName()
        {
            var parameters = parser.Parse(@"foo");

            var result = binder.CanBind<CopyCommand>(parameters);

            Assert.IsFalse(result.Bound);

            Assert.AreEqual(0, result.TotalBound);
            Assert.AreEqual(3, result.TotalUnbound);

            Assert.AreEqual(1, result.UnknownVerbs.Count);
            Assert.AreEqual("foo", result.UnknownVerbs[0].Name);
        }

        /*

        [Test]
        public void TestBindSingleVerbsCommandWhenBound()
        {
            var parameters = parser.Parse("copy from to");

            var bound = binder.TryBind<SingleVerbsCommand>(parameters, out var command);

            Assert.IsTrue(bound);
            Assert.IsNotNull(command);
        }*/
    }
}
