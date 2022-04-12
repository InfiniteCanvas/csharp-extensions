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
        public void FromNullablesWorks()
        {
            Assert.IsInstanceOf<None<Random>>(GetDefault<Random>().MaybeNull());
            Assert.IsInstanceOf<Some<int>>(GetDefault<int>().MaybeNull());

            Assert.IsInstanceOf<Some<int>>(new Nullable<int>(2).MaybeNull());
            Assert.IsInstanceOf<None<int>>(new Nullable<int>().MaybeNull());
        }

        private T ThrowError<T>() => throw new Exception();
        private T GetDefault<T>() => default(T);
    }
}