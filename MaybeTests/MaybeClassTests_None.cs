using Common;
using NUnit.Framework;

namespace MaybeTests;

public class MaybeClassTests_None
{
    [Test]
    public void EqualityChecks()
    {
        var none1 = None<int>.Instance;
        var none2 = None<int>.Instance;
        
        Assert.AreEqual(none1, none2);
        Assert.AreEqual(none1.GetHashCode(), none2.GetHashCode());
        Assert.AreEqual(none1.ToString(), none2.ToString());
        Assert.IsTrue(none1 == none2);
        Assert.IsFalse(none1 != none2);
        Assert.IsTrue(none1.Equals(none2));
        Assert.IsTrue(none1.Equals((object)none2));
        Assert.IsFalse(none1.Equals(null));
    }

    [Test]
    public void Implicit_NewNoneInstance()
    {
        None<int> none = 1;
        Assert.IsInstanceOf<None<int>>(none);
        Assert.AreEqual(none, None<int>.Instance);
    }
}