using System;
using NUnit.Framework;

namespace Contastic
{
    [TestFixture]
    public class ParameterBinderTest
    {
        private class TestClass
        {
            [Parameter(Default = "John")]
            public string Name { get; set; }

            [Parameter]
            public int Age { get; set; }

            [Parameter(Name = "dob")]
            public DateTime DateOfBirth { get; set; }

            [Parameter]
            public int ReadOnly { get { return 3; } }

            [Flag("awake")]
            public bool Awake { get; set; }
        }

        [Flag("required")]
        private class TestRequiredClass
        {
            [Parameter(Required = true)]
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

        [Test]
        public void TestBindObjectCaseInsensitive()
        {
            var parameters = parser.Parse("-Name Jim");

            var result = binder.Bind<TestClass>(parameters);

            Assert.IsNotNull(result);
            Assert.AreEqual("Jim", result.Name);
        }

        [Test]
        public void TestBindObjectToInteger()
        {
            var parameters = parser.Parse("-age 30");

            var result = binder.Bind<TestClass>(parameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(30, result.Age);
        }

        [Test]
        public void TestBindObjectToDateTime()
        {
            var parameters = parser.Parse("-dob 1980-06-01");

            var result = binder.Bind<TestClass>(parameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DateOfBirth.Day);
            Assert.AreEqual(6, result.DateOfBirth.Month);
            Assert.AreEqual(1980, result.DateOfBirth.Year);
        }

        [Test]
        public void TestBindObjectToDateTimeWhenIncorrectFormat()
        {
            var parameters = parser.Parse("-dob hello");

            try
            {
                binder.Bind<TestClass>(parameters);

                Assert.Fail("Should have blown up!");
            }
            catch (BindingException exception)
            {
                Assert.IsNotNull(exception);
                Assert.AreEqual("Could not convert 'hello' to type System.DateTime", exception.Message);
            }
        }

        [Test]
        public void TestDefaultParameterValue()
        {
            var result = binder.Bind<TestClass>("");

            Assert.AreEqual("John", result.Name);
        }

        [Test]
        public void TestBindRequiredParameter()
        {
            var result = binder.Bind<TestRequiredClass>("-name Jane -required");

            Assert.AreEqual("Jane", result.Name);
        }

        [Test]
        public void TestBindWhenRequiredParameterMissing()
        {
            try
            {
                binder.Bind<TestRequiredClass>("");

                Assert.Fail("Should have blown up!");
            }
            catch (BindingException exception)
            {
                Assert.IsNotNull(exception);
                Assert.AreEqual("Couldn't bind to object, missing required parameter: name", exception.Message);
            }
        }

        [Test]
        public void TestBindFlags()
        {
            var result = binder.Bind<TestClass>("-awake");

            Assert.IsTrue(result.Awake);
        }

        [Test]
        public void TestBindFlagsWhenMissing()
        {
            var result = binder.Bind<TestClass>("");

            Assert.IsFalse(result.Awake);
        }

        [Test]
        public void TestTryBindWhenBindable()
        {
            TestClass result;

            var success = binder.TryBind("-name Chris", out result);

            Assert.IsTrue(success);
            Assert.AreEqual("Chris", result.Name);
        }

        [Test]
        public void TestTryBindWhenNotBindable()
        {
            TestRequiredClass result;

            var success = binder.TryBind("", out result);

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }
    }
}
