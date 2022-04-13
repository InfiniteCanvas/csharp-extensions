using NUnit.Framework;

namespace Common.Extensions.Tests;

[TestFixture]
public class OptionsTests
{
    [Test]
    public void ConvertsToSomeAndNoneProperly()
    {
        Assert.IsInstanceOf<Option<int>>(1.Some());
        Assert.IsInstanceOf<Option<int>>(1.None());
    }

    [Test]
    public void Some_ToString()
    {
        var val = new Some<float>(0.3f);
        var str = val.ToString();
        Assert.IsInstanceOf<string>(str);
    }

    [Test]
    public void Some_Equalities()
    {
        var some1 = "test".Some();
        var some2 = "test".Some();
        var some3 = "no".Some();
        var none = "test".None();

        Assert.AreEqual(some1, some2);
        Assert.IsTrue(some1  == some2);
        Assert.IsFalse(some1 != some2);

        Assert.AreNotEqual(some1, some3);
        Assert.IsFalse(some1 == some3);
        Assert.IsTrue(some1  != some3);

        Assert.AreNotEqual(some1, none);
        Assert.IsFalse(some1 == none);
        Assert.IsTrue(some1  != none);
    }

    [Test]
    public void None_Equalities()
    {
        var some = 5.4.Some();
        var none = 5.4.None();
        var none2 = 2.1.None();
        var none3 = 15.None();
        Assert.AreNotEqual(some, none);
        Assert.IsTrue(some  != none);
        Assert.IsFalse(some == none);

        Assert.AreEqual(none, none2);
        Assert.IsTrue(none2  == none);
        Assert.IsFalse(none2 != none);

        Assert.AreNotEqual(none, none3);
    }

    [Test]
    public void Implicit_TValue()
    {
        Option<int> a = 2;
        Assert.AreEqual(2, (int) a);
    }
}
