using NUnit.Framework;

namespace Contastic
{
    [TestFixture]
    public class ParameterBinderTest
    {
        private class TestClass
        {
            [Parameter("name")]
            public string Name { get; set; }
        }

        private ParameterBinder binder;
        private ParameterParser parser;

        [SetUp]
        public void SetUp()
        {
            binder = new ParameterBinder();
            parser = new ParameterParser();
        }

        [Test]
        public void TestBindObject()
        {
            var parameters = parser.Parse("-name Jim");

            var result = binder.Bind<TestClass>(parameters);

            Assert.IsNotNull(result);
            Assert.AreEqual("Jim", result.Name);
        }
    }
}
