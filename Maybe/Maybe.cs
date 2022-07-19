using System.Diagnostics.CodeAnalysis;

namespace Common;

public abstract class Maybe<TSource> : IEquatable<Maybe<TSource>>
{
    public bool Equals(Maybe<TSource>? other) =>
        this switch
        {
            Some<TSource> some1 when other is Some<TSource> some2 => some1.Equals(some2),
            None<TSource> when other is None<TSource>             => true,
            _                                         => false,
        };

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Maybe<TSource> other && Equals(other);

    public override int GetHashCode() =>
        this switch
        {
            Some<TSource> some => some.GetHashCode(),
            _            => 0,
        };

    public static bool operator ==(Maybe<TSource>? left, Maybe<TSource>? right) =>
        (left, right) switch
        {
            (Some<TSource> some1, Some<TSource> some2) => some1.Equals(some2),
            (None<TSource>, None<TSource>)             => true,
            _                              => false,
        };

    public static bool operator !=(Maybe<TSource>? left, Maybe<TSource>? right) =>
        (left, right) switch
        {
            (Some<TSource> some1, Some<TSource> some2) => !some1.Equals(some2),
            (None<TSource>, None<TSource>)             => false,
            _                              => true,
        };
}

public sealed class Some<TSource> : Maybe<TSource>, IEquatable<Some<TSource>>
{
    [NotNull] public TSource Value { get; }

    public Some([DisallowNull] TSource value)
    {
        if (value is null) throw new ArgumentNullException($"{nameof(value)} must not be null");
        Value = value;
    }

    public bool Equals(Some<TSource>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || EqualityComparer<TSource>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Some<TSource> other && Equals(other);

    public override int GetHashCode() => EqualityComparer<TSource>.Default.GetHashCode(Value);

    private static bool Equals(Some<TSource>? left, Some<TSource>? right) =>
        left is not null && right is not null && left.Equals(right);

    public static bool operator ==(Some<TSource>? left, Some<TSource>? right) => Equals(left, right);

    public static bool operator !=(Some<TSource>? left, Some<TSource>? right) => !Equals(left, right);

    public override string ToString() => Value.ToString() ?? string.Empty;

    public static implicit operator TSource(Some<TSource> value) => value.Value;
}

public sealed class None<TSource> : Maybe<TSource>, IEquatable<None<TSource>>
{
    public static None<TSource> Instance { get; } = new();

    private None() { }

    public override string ToString() => string.Empty;

    public static implicit operator None<TSource>(TSource _) { return Instance; }

    public bool Equals(None<TSource>? other) =>
        other switch
        {
            not null => true,
            _        => ReferenceEquals(this, other),
        };

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is None<TSource> other && Equals(other);

    public override int GetHashCode() => 0;

    private static bool Equals(None<TSource>? left, None<TSource>? right) => left is not null && right is not null;

    public static bool operator ==(None<TSource>? left, None<TSource>? right) => Equals(left, right);

    public static bool operator !=(None<TSource>? left, None<TSource>? right) => !Equals(left, right);
}