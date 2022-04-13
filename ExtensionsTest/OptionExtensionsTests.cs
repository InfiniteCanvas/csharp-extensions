using System;
using System.Security.Principal;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Common.Extensions.Tests;

[TestFixture]
public class OptionsExtensionsTests
{
    [Test]
    public void Some_FromNull()
    {
        object a = null;
        var b = a.Some();

        Console.WriteLine("Type should not be Some when fetching from null");
        Console.WriteLine(b.GetType());
        
        Assert.IsNotInstanceOf<Some<object>>(b);
    }
    
    [Test]
    public void TryCatch_Some_OnlyFunc()
    {
        var m = GetDefault<string>;
        Assert.DoesNotThrow(() => m.TryCatch());
    }

    [Test]
    public void TryCatch_None_OnlyFunc()
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

    [Test]
    public void FromMaybeNull_And_IsSome_IsNone_Test()
    {
        object a = null;
        object b = new object();
        int c = 1;
        
        Assert.IsTrue(a.FromMaybeNull().IsNone());
        Assert.IsTrue(b.FromMaybeNull().IsSome());
        Assert.IsTrue(c.FromMaybeNull().IsSome());
    }

    [Test]
    public void TryBind_Some()
    {
        var a = "2".Some();
        var b = a.TryBind(x => int.Parse(x).Some());

        Console.WriteLine($"Binding {a} to Option<int> with parsing function");
        Console.WriteLine($"Object: {b}\nValue: {b.TryGet()}");
        
        Assert.AreEqual(2, b.TryGet());
    }

    [Test]
    public void MaybeMap_Some()
    {
        Console.WriteLine("Should return 'Some' if some and 'None' if none");
        var some = "Some".Some();
        var none = "None".None();
        var ifsome = new Func<string, string>(x => "Some");
        var ifnone = new Func<string>(() => "None");

        var done = some.MaybeMap(ifsome, ifnone);
        var didnt = none.MaybeMap(ifsome, ifnone);
        
        Assert.AreEqual(done.TryGet(), "Some");
        Assert.AreEqual(didnt.TryGet(), "None");
    }
#region Helper functions

    private void ExplicitAssignmentWithThrow()
    {
        Option<int> start = new None<int>();
        var end = (string) start.TryMap(x => x.ToString());
    }

    private T ThrowError<T>() => throw new Exception();

    private T? GetDefault<T>() => default;

#endregion
}
