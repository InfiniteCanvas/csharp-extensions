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

    public override int GetHashCode() => this is Some<TValue> some ? some.Value.GetHashCode() : 0;

    public static bool operator ==(Option<TValue> opt1, Option<TValue> opt2) => Equals(opt1, opt2);

    public static bool operator !=(Option<TValue> opt1, Option<TValue> opt2) => !Equals(opt1, opt2);

    public static implicit operator Option<TValue>([DisallowNull] TValue value) => new Some<TValue>(value);

    public static explicit operator TValue(Option<TValue> option) => option.TryGet();
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
