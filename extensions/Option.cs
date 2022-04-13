using System.Diagnostics.CodeAnalysis;

namespace Common.Extensions;

/// <summary>
///     Represents some value or none
/// </summary>
/// <typeparam name="TValue">The wrapped type</typeparam>
public abstract class Option<TValue>
{
    public override bool Equals(object obj) =>
        this switch
        {
            Some<TValue> some1 when obj is Some<TValue> some2 => Equals(some1.Value, some2.Value),
            None<TValue> _ when obj is None<TValue> _         => true,
            _                                                 => false,
        };

    public override int GetHashCode() =>
        this switch
        {
            Some<TValue> some => some.Value.GetHashCode(),
            None<TValue> _    => 0,
            _                 => throw new ArgumentOutOfRangeException()
        };

    public static bool operator ==(Option<TValue> opt1, Option<TValue> opt2) => Equals(opt1, opt2);

    public static bool operator !=(Option<TValue> opt1, Option<TValue> opt2) => !Equals(opt1, opt2);

    public static implicit operator Option<TValue>([DisallowNull] TValue value) => new Some<TValue>(value);

    public static explicit operator TValue(Option<TValue> option) =>
        option switch
        {
            Some<TValue> some => some.Value,
            _                 => throw new Exception(),
        };
}

/// <summary>
///     Some value is there
/// </summary>
/// <typeparam name="TValue">The wrapped type</typeparam>
public sealed class Some<TValue> : Option<TValue>
{
    public Some([DisallowNull, NotNull] TValue value) => Value = value;

    [NotNull]
    public TValue Value { get; }

    public static implicit operator TValue(Some<TValue> value) => value.Value;

    public override string ToString() => Value.ToString();
}

/// <summary>
///     No value is there
/// </summary>
/// <typeparam name="TValue">The wrapped type</typeparam>
public sealed class None<TValue> : Option<TValue>
{
    public override string ToString() => throw new Exception();
}

public sealed class None
{
    private static None _value;

    private None() { }

    public static None Instance => _value ??= new None();
}
