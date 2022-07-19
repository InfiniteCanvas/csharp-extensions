using System;
using System.Diagnostics.CodeAnalysis;
using Common;
using NUnit.Framework;

namespace MaybeTests;

public class MaybeClassTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ImplicitConversion_Struct()
    {
        var maybe = new Some<int>(1);
        int someInt = maybe;
        Assert.IsInstanceOf<int>(someInt);
    }

    [Test]
    public void ImplicitConversion_Class()
    {
        var maybe = new Some<object>(new object());
        object someObject = maybe;
        Assert.IsInstanceOf<object>(someObject);
    }

    [Test]
    public void Ctor_FromNull()
    {
        Assert.Throws<ArgumentNullException>(CreateFromNull);
    }
    
    [ExcludeFromCodeCoverage]
    private void CreateFromNull()
    {
        try
        {
            var fail = new Some<object>(null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}