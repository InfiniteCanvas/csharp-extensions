using Common.Extensions;
using NUnit.Framework;
using System;

namespace OptionExtensionsTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TryCatchSome()
        {
            var m = GetDefault<string>;
            Assert.DoesNotThrow(() => m.TryCatch());
        }

        [Test]
        public void TryCatchNone()
        {
            var m = ThrowError<string>;
            Assert.IsInstanceOf<None<string>>(m.TryCatch());
        }

        [Test]
        public void ConvertsToSomeAndNoneProperly()
        {
            Assert.IsInstanceOf<Option<int>>(1.Some());
            Assert.IsInstanceOf<Option<int>>(1.None());
        }

        [Test]
        public void OptionMap_SomeChecks()
        {
            // some
            Option<int> start = new Some<int>(0);
            var end = start.TryMap(x => x.ToString());
            Assert.AreEqual("0".Some(), end);
        }

        [Test]
        public void OptionMap_SomeChecks_ImplicitOperator()
        {
            // some
            Option<int> start = new Some<int>(0);
            string end = start.TryMap(x => x.ToString());
            Assert.AreEqual("0", end);
        }

        [Test]
        public void OptionMap_NoneChecks()
        {
            // some
            Option<int> start = new None<int>();
            var end = start.TryMap(x => x.ToString());
            Assert.AreEqual(new None<string>(), end);
        }

        [Test]
        public void OptionMap_NoneChecks_Implicit()
        {
            Assert.Throws<Exception>(ImplicitAssignmentWithThrow);
        }

        private void ImplicitAssignmentWithThrow()
        {
            Option<int> start = new None<int>();
            string end = start.TryMap(x => x.ToString());
        }

        private T ThrowError<T>() => throw new Exception();
        private T? GetDefault<T>() => default;
    }
}