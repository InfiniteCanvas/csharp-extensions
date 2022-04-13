using System;
using NUnit.Framework;

namespace Common.Extensions.Tests;

[TestFixture]
public class OptionsExtensionsTests
{
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
    public void OptionMap_SomeChecks()
    {
        // some
        Option<int> start = new Some<int>(0);
        var end = start.TryMap(x => x.ToString());
        Assert.AreEqual("0".Some(), end);
    }

    [Test]
    public void OptionMap_SomeChecks_ExplicitOperator()
    {
        // some
        Option<int> start = new Some<int>(0);
        var end = (string) start.TryMap(x => x.ToString());
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
        Assert.Throws<Exception>(ExplicitAssignmentWithThrow);
    }

    private void ExplicitAssignmentWithThrow()
    {
        Option<int> start = new None<int>();
        var end = (string) start.TryMap(x => x.ToString());
    }

    private T ThrowError<T>() => throw new Exception();

    private T? GetDefault<T>() => default;
}
