using System.Diagnostics.CodeAnalysis;

namespace Common.Extensions;

/// <summary>
///     Represents some value or none
/// </summary>
/// <typeparam name="TValue">The wrapped type</typeparam>
public abstract class Option<TValue>
{
    public override bool Equals(object obj)
    {
        return this switch
        {
            Some<TValue> some1 when obj is Some<TValue> some2 => Equals(some1.Value, some2.Value),
            None<TValue> _ when obj is None<TValue> _ => true,
            _ => false,
        };
    }

    public override int GetHashCode()
    {
        return this switch
        {
            Some<TValue> some => some.GetHashCode(),
            None<TValue> _ => 0,
            _ => throw new Exception("Invalid type"),// Here for the compiler. Should never happen
        };
    }

    public static bool operator ==(Option<TValue> opt1, Option<TValue> opt2) =>
        Equals(opt1, opt2);

    public static bool operator !=(Option<TValue> opt1, Option<TValue> opt2) =>
        !Equals(opt1, opt2);

    public static implicit operator Option<TValue>([DisallowNull] TValue value) => new Some<TValue>(value);
    public static implicit operator None(Option<TValue> _) => None.Instance;
    public static implicit operator TValue(Option<TValue> option) => option switch
    {
        Some<TValue> some => some,
        None<TValue> _ => throw new Exception(),
        _ => throw new Exception()
    };
}

/// <summary>
///     Some value is there
/// </summary>
/// <typeparam name="TValue">The wrapped type</typeparam>
public sealed class Some<TValue> : Option<TValue>
{
    [NotNull]
    public TValue Value { get; }
    public Some([DisallowNull, NotNull] TValue value) => Value = value;
    public static implicit operator TValue(Some<TValue> value) => value.Value;

    public override string? ToString() => Value.ToString();
}

/// <summary>
///     No value is there
/// </summary>
/// <typeparam name="TValue">The wrapped type</typeparam>
public sealed class None<TValue> : Option<TValue>
{
    public override string ToString()
    {
        throw new();
    }
}

public sealed class None
{
    public static None Instance => _value ??= new();
    private static None? _value;
    private None() { }
}