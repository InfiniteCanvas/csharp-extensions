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
        public void MaybeNullFromClasses_SomeAndNoneCases()
        {
            Assert.IsInstanceOf<None<Random>>(GetDefault<Random>().MaybeNull());
            var t = new Object();
            Assert.IsInstanceOf<Some<object>>(t.MaybeNull());
        }

        [Test]
        public void MaybeNullFromNullable_SomeAndNoneCases()
        {
            Assert.IsInstanceOf<Some<int>>(new Nullable<int>(2).MaybeNull());
            Assert.IsInstanceOf<None<int>>(new Nullable<int>().MaybeNull());
        }

        [Test]
        public void OptionMap_SomeChecks()
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
            string end = start.TryMap(x => x.ToString());
            Assert.AreEqual("0", end);
        }

        private T ThrowError<T>() => throw new Exception();
        private T? GetDefault<T>() => default;
    }
}