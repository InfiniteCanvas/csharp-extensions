#region

using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

#endregion

namespace Common.Extensions.Tests;

[TestFixture]
public class OptionsExtensionsTests
{
    public void TryCatch_Map_Some()
    {
        var m = GetDefault<string>;
        Assert.DoesNotThrow(() => m.TryCatch());
    }

    public void TryCatch_Map_None()
    {
        var m = ThrowError<string>;
        Assert.IsInstanceOf<None<string>>(m.TryCatch());
    }

    private void TryCatch_Bind_Some()
    {
        var some = "2".Some();
        var func = new Func<string, Option<int>>(x => int.Parse(x).Some());
        Assert.DoesNotThrow(() => some.TryCatch(func));
    }

    private void TryCatch_Bind_None()
    {
        var some = "2".None();
        var func = new Func<string, Option<int>>(ParseToOptionInt);
        Assert.DoesNotThrow(() => some.TryCatch(func));
    }

    [ExcludeFromCodeCoverage]
    private Option<int> ParseToOptionInt(string val) => int.Parse(val).Some();

    [ExcludeFromCodeCoverage]
    private T Return<T>(T val) => val;

    [ExcludeFromCodeCoverage]
    private void ExplicitAssignmentWithThrow()
    {
        Option<int> start = new None<int>();
        var end = (string) start.TryMap(x => x.ToString());
    }

    [ExcludeFromCodeCoverage]
    private string UseToString<T>(T val) => val?.ToString() ?? throw new Exception();

    [ExcludeFromCodeCoverage]
    private T ThrowError<T>() => throw new Exception();

    [ExcludeFromCodeCoverage]
    private T? GetDefault<T>() => default;

    [Test]
    public void FromMaybeNull_And_IsSome_IsNone_Test()
    {
        object a = null;
        var b = new object();
        var c = 1;

        Assert.IsTrue(a.FromMaybeNull().IsNone());
        Assert.IsTrue(b.FromMaybeNull().IsSome());
        Assert.IsTrue(c.FromMaybeNull().IsSome());
    }

    [Test]
    public void MaybeDo()
    {
        var val = "";
        var @do = new Action<string>(x => val = x);
        var dont = new Action(() => val = "None");

        var some = "Some".Some();
        var none = "2".None();

        some.MaybeDo(@do, dont);
        Assert.AreEqual(val, "Some");

        none.MaybeDo(@do, dont);
        Assert.AreEqual(val, "None");
    }

    [Test]
    public void MaybeDo_Throw()
    {
        var @do = new Action<string>(x => { Console.WriteLine($"{x} works fine"); });
        var dont = new Action(() => throw new Exception());
        var none = "2".None();

        Assert.Throws<Exception>(() => none.MaybeDo(@do, dont));
    }

    [Test]
    public void MaybeDoBind()
    {
        var @do = new Func<string, Option<string>>(x => x.Some());
        var dont = new Func<Option<string>>(() => "None".None());

        var some = "Some".Some();
        var none = "2".None();

        Assert.AreEqual(some.MaybeDoBind(@do, dont), "Some".Some());
        Assert.AreEqual(none.MaybeDoBind(@do, dont), "None".None());
    }

    [Test]
    public void MaybeDoBind_Throw()
    {
        var @do = new Func<string, Option<string>>(OptionExtensions.Some);
        var dont = new Func<Option<string>>(() => throw new Exception());

        var none = "Some".None();

        Assert.Throws<Exception>(() => none.MaybeDoBind(@do, dont));
    }

    [Test]
    public void MaybeMap_Some()
    {
        Console.WriteLine("Should return 'Some' if some and 'None' if none");
        var some = "Some".Some();
        var none = "None".None();
        var ifsome = new Func<string, string>(x => x);
        var ifnone = new Func<string>(() => "None");

        var done = some.MaybeMap(ifsome, ifnone);
        var didnt = none.MaybeMap(ifsome, ifnone);

        Assert.AreEqual(done.TryGet(), "Some");
        Assert.Throws<Exception>(() => didnt.TryGet());
    }

    [Test]
    public void OptionMap_NoneChecks()
    {
        // some
        Option<int> start = new None<int>();
        var end = start.TryMap(UseToString);
        Assert.AreEqual(new None<string>(), end);
    }

    [Test]
    public void OptionMap_NoneChecks_Implicit()
    {
        Assert.Throws<Exception>(ExplicitAssignmentWithThrow);
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
    public void Some_FromNull()
    {
        object a = null;
        var b = a.Some();

        Console.WriteLine("Type should not be Some when fetching from null");
        Console.WriteLine(b.GetType());

        Assert.IsNotInstanceOf<Some<object>>(b);
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
    public void TryCatch_Bind()
    {
        void TestDelegate()
        {
            TryCatch_Bind_Some();
            TryCatch_Bind_None();
        }

        Assert.Multiple(TestDelegate);
    }

    [Test]
    public void TryCatch_Map()
    {
        void TestDelegate()
        {
            TryCatch_Map_None();
            TryCatch_Map_Some();
        }

        Assert.Multiple(TestDelegate);
    }
}
