using System.Diagnostics.CodeAnalysis;

namespace Common;

/// <summary>
///     A simple wrapper around a value that can be Some or None.
/// </summary>
/// <typeparam name="TSource"> The type of the value in the maybe. </typeparam>
public abstract class Maybe<TSource> : IEquatable<Maybe<TSource>>
{
    /// <summary>
    ///     Implements the interface of <see cref="IEquatable{T}" />.
    ///     Compares two Maybe values for equality. When both values are <see cref="None{TSource}" />, they are considered
    ///     equal.
    ///     When both values are <see cref="Some{TSource}" />, they are compared for equality with the
    ///     <see cref="Some{TSource}" /> Equals method.
    /// </summary>
    /// <param name="other"> The other Maybe value to compare with this value. </param>
    /// <returns> True if the values are equal, false otherwise. </returns>
    public bool Equals(Maybe<TSource>? other) =>
        this switch
        {
            Some<TSource> some1 when other is Some<TSource> some2 => some1.Equals(some2),
            None<TSource> when other is None<TSource>             => true,
            _                                                     => false,
        };

    /// <summary>
    ///     Compares this object with another object for equality.
    /// </summary>
    /// <param name="obj"> The object to compare with this object. </param>
    /// <returns>
    ///     True for reference equality, and if the other object is a <see cref="Maybe{TSource}" />, then the result of
    ///     <see cref="Equals(Maybe{TSource})" />.
    /// </returns>
    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || (obj is Maybe<TSource> other && Equals(other));

    /// <summary>
    ///     Gets the hash code for this object.
    /// </summary>
    /// <returns>
    ///     If this value is <see cref="Some{TSource}" />, the hash code of the <see cref="Some{TSource}" /> value.
    ///     Otherwise, the hash code of <see cref="None{TSource}"/>.
    /// </returns>
    public override int GetHashCode() =>
        this switch
        {
            Some<TSource> some => some.GetHashCode(),
            _                  => None<TSource>.Instance.GetHashCode()
        };

    /// <summary>
    /// String representation of the value.
    /// </summary>
    /// <returns> If this value is <see cref="Some{TSource}" />, the string representation of the <see cref="Some{TSource}" /> value. Else, the string representation of <see cref="None{TSource}"/>. </returns>
    public override string? ToString() =>
        this switch
        {
            Some<TSource> some => some.ToString(),
            _                  => None<TSource>.Instance.ToString(),
        };

    /// <summary>
    ///     Overrides the == operator to compare two Maybe values for equality. When both values are
    ///     <see cref="None{TSource}" />, they are considered equal.
    ///     When both values are <see cref="Some{TSource}" />, they are compared for equality with the
    ///     <see cref="Some{TSource}" /> Equals method.
    /// </summary>
    /// <param name="left"> The left Maybe value to compare. </param>
    /// <param name="right"> The right Maybe value to compare. </param>
    /// <returns> True if the values are equal, false otherwise. </returns>
    public static bool operator ==(Maybe<TSource>? left, Maybe<TSource>? right) =>
        (left, right) switch
        {
            (Some<TSource> some1, Some<TSource> some2) => some1.Equals(some2),
            (None<TSource>, None<TSource>)             => true,
            _                                          => false,
        };

    /// <summary>
    ///     Overrides the != operator to compare two Maybe values for inequality. When both values are
    ///     <see cref="None{TSource}" />, they are considered equal.
    ///     When both values are <see cref="Some{TSource}" />, they are compared for inequality with the
    ///     <see cref="Some{TSource}" /> Equals method.
    /// </summary>
    /// <param name="left"> The left Maybe value to compare. </param>
    /// <param name="right"> The right Maybe value to compare. </param>
    /// <returns> True if the values are not equal, false otherwise. </returns>
    public static bool operator !=(Maybe<TSource>? left, Maybe<TSource>? right) =>
        (left, right) switch
        {
            (Some<TSource> some1, Some<TSource> some2) => !some1.Equals(some2),
            (None<TSource>, None<TSource>)             => false,
            _                                          => true,
        };
}

/// <summary>
///     A simple wrapper around a value and it is never null.
/// </summary>
/// <typeparam name="TSource"> The type of the wrapped value. </typeparam>
public sealed class Some<TSource> : Maybe<TSource>, IEquatable<Some<TSource>>
{
    /// <summary>
    ///     Wrapped value. Can never be null.
    /// </summary>
    [NotNull]
    public TSource Value => _value;

    [NotNull] private readonly TSource _value;

    /// <summary>
    /// Creates a new Some value. Input cannot be null.
    /// </summary>
    /// <param name="value"> Value to wrap. </param>
    /// <exception cref="ArgumentNullException"> Thrown if value is null. </exception>
    public Some([DisallowNull] TSource value)
    {
        if (value is null) throw new ArgumentNullException($"{nameof(value)} must not be null");
        _value = value;
    }

    /// <summary>
    /// Compares two Some values for equality using the <see cref="EqualityComparer{TSource}"/> of <typeparamref name="TSource"/>.
    /// </summary>
    /// <param name="other"> The other Some value to compare with this value. </param>
    /// <returns> True if the values are equal, false otherwise. </returns>
    public bool Equals(Some<TSource>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || EqualityComparer<TSource>.Default.Equals(Value, other.Value);
    }

    /// <summary>
    /// Compares this object with another object for equality.
    /// Uses the <see cref="Equals(Some{TSource})"/> method if the other object is a <see cref="Some{TSource}"/>.
    /// </summary>
    /// <param name="obj"> The object to compare with this object. </param>
    /// <returns> True for equal references, and if the other object is a <see cref="Some{TSource}"/>, then the result of <see cref="Equals(Some{TSource})"/>. </returns>
    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || (obj is Some<TSource> other && Equals(other));

    /// <summary>
    /// Gets the hash code for this object.
    /// </summary>
    /// <returns> The combined hash code of this instance and the wrapped value. </returns>
    public override int GetHashCode() => HashCode.Combine(this, _value);

    /// <summary>
    /// Compares two Some values for equality.
    /// </summary>
    /// <param name="left"> The left Some value to compare. </param>
    /// <param name="right"> The right Some value to compare. </param>
    /// <returns> True if the values are equal, false otherwise. </returns>
    private static bool Equals(Some<TSource>? left, Some<TSource>? right) =>
        left is not null && right is not null && left.Equals(right);

    /// <summary>
    /// Overrides the == operator to compare two Some values for equality.
    /// </summary>
    /// <param name="left"> The left Some value to compare. </param>
    /// <param name="right"> The right Some value to compare. </param>
    /// <returns> True if the values are equal, false otherwise. </returns>
    public static bool operator ==(Some<TSource>? left, Some<TSource>? right) => Equals(left, right);

    /// <summary>
    /// Overrides the != operator to compare two Some values for inequality.
    /// </summary>
    /// <param name="left"> The left Some value to compare. </param>
    /// <param name="right"> The right Some value to compare. </param>
    /// <returns> True if the values are not equal, false otherwise. </returns>
    public static bool operator !=(Some<TSource>? left, Some<TSource>? right) => !Equals(left, right);

    /// <summary>
    /// Returns the ToString() result of the wrapped value.
    /// </summary>
    /// <returns> The ToString() result of the wrapped value. </returns>
    public override string? ToString() => _value.ToString();

    /// <summary>
    /// Implicit extraction of the wrapped value.
    /// </summary>
    /// <param name="value"> The Some value to extract. </param>
    /// <returns> The wrapped value. </returns>
    public static implicit operator TSource(Some<TSource> value) => value.Value;
}

/// <summary>
/// A simple wrapper around <typeparamref name="TSource"/> that represents no value exists.
/// </summary>
/// <typeparam name="TSource"> The type of the None wrapper. </typeparam>
public sealed class None<TSource> : Maybe<TSource>, IEquatable<None<TSource>>
{
    // The None value is always the same instance for the same type.
    private None() { }

    /// <summary>
    /// Gets the singleton instance of the None value for the given type.
    /// </summary>
    public static None<TSource> Instance { get; } = new();

    /// <summary>
    /// Compares two None values for equality.
    /// </summary>
    /// <param name="other"> The other None value to compare with this value. </param>
    /// <returns> True if both are <see cref="None{TSource}"/>, false otherwise. </returns>
    public bool Equals(None<TSource>? other) =>
        other switch
        {
            not null => true,
            _        => false,
        };

    /// <summary>
    /// Returns an empty string.
    /// </summary>
    /// <returns> string.Empty </returns>
    public override string? ToString() => string.Empty;

    /// <summary>
    /// Implicitly converts a <typeparamref name="TSource"/> to a <see cref="None{TSource}"/>.
    /// </summary>
    /// <param name="_"> Arbitrary value of type <typeparamref name="TSource"/>. </param>
    /// <returns> The <see cref="None{TSource}"/> instance. </returns>
    public static implicit operator None<TSource>(TSource _) => Instance;

    /// <summary>
    /// Compares two objects for equality.
    /// </summary>
    /// <param name="obj"> The object to compare with this object. </param>
    /// <returns> True for reference equality, or if the other object is a <see cref="None{TSource}"/>, otherwise false. </returns>
    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || (obj is None<TSource> other && Equals(other));

    /// <summary>
    /// Gets the hash code for the singleton instance of type <see cref="None{TSource}"/>.
    /// </summary>
    /// <returns> Integer hash code for the singleton instance of type <see cref="None{TSource}"/>. </returns>
    public override int GetHashCode() => HashCode.Combine(Instance);

    /// <summary>
    /// Compares two None values for equality.
    /// </summary>
    /// <param name="left"> The left None value to compare. </param>
    /// <param name="right"> The right None value to compare. </param>
    /// <returns> True if ReferenceEquals or both are <see cref="None{TSource}"/>, otherwise false. </returns>
    private static bool Equals(None<TSource>? left, None<TSource>? right) =>
        ReferenceEquals(left, right) || left is not null && right is not null;

    /// <summary>
    /// Overrides the == operator to compare two None values for equality.
    /// </summary>
    /// <param name="left"> The left None value to compare. </param>
    /// <param name="right"> The right None value to compare. </param>
    /// <returns> True if ReferenceEquals or both are <see cref="None{TSource}"/>, otherwise false. </returns>
    public static bool operator ==(None<TSource>? left, None<TSource>? right) => Equals(left, right);

    /// <summary>
    /// Overrides the != operator to compare two None values for inequality.
    /// </summary>
    /// <param name="left"> The left None value to compare. </param>
    /// <param name="right"> The right None value to compare. </param>
    /// <returns> False if ReferenceEquals or both are <see cref="None{TSource}"/>, otherwise true. </returns>
    public static bool operator !=(None<TSource>? left, None<TSource>? right) => !Equals(left, right);
}