using System.Linq;
using Contastic.Models;
using Contastic.Samples;
using NUnit.Framework;

namespace Contastic.Binding
{
    [TestFixture]
    public class CanBindResultTests
    {
        [Test]
        public void TestHelpTextWithOneArgument()
        {
            var binding = new CanBindResult();
            var propertyInfo = typeof(CopyCommand).GetProperties().First();
            binding.AddBoundArgument(new ArgumentAttribute { Name = "foo" }, new Token(), propertyInfo);

            Assert.AreEqual("<foo>", binding.ToHelpText());
        }

        [Test]
        public void TestHelpTextWithTwoArgument()
        {
            var binding = new CanBindResult();
            var propertyInfo = typeof(CopyCommand).GetProperties().First();
            binding.AddBoundArgument(new ArgumentAttribute { Name = "foo" }, new Token(), propertyInfo);
            binding.AddBoundArgument(new ArgumentAttribute { Name = "bar" }, new Token(), propertyInfo);

            Assert.AreEqual("<bar> <foo>", binding.ToHelpText());
        }

        [Test]
        public void TestHelpTextWithOneArgumentAndOption()
        {
            var binding = new CanBindResult();
            var propertyInfo = typeof(CopyCommand).GetProperties().First();
            var token = new Token();
            token.AppendName("user");
            binding.AddBoundArgument(new ArgumentAttribute { Name = "foo" }, new Token(), propertyInfo);
            binding.AddBoundOption(propertyInfo, new OptionAttribute{ Required = true },  token, "");

            Assert.AreEqual("<foo> --user", binding.ToHelpText());
        }

        [Test]
        public void TestHelpTextWithOneArgumentAndOptionalOption()
        {
            var binding = new CanBindResult();
            var propertyInfo = typeof(CopyCommand).GetProperties().First();
            var token = new Token();
            token.AppendName("user");
            binding.AddBoundArgument(new ArgumentAttribute { Name = "foo" }, new Token(), propertyInfo);
            binding.AddBoundOption(propertyInfo, new OptionAttribute{ Required = false }, token, "");

            Assert.AreEqual("<foo> [--user]", binding.ToHelpText());
        }
    }
}
