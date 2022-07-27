using System.Diagnostics.CodeAnalysis;

namespace Common.Extensions;

/// <summary>
/// Extends the Maybe type with some useful methods.
/// </summary>
public static class MaybeExtensions
{
    /// <summary>
    /// Creates a <see cref="Some{TSource}"/> from the given value. Value cannot be null.
    /// </summary>
    /// <param name="value"> The value to wrap. </param>
    /// <typeparam name="TSource"> The type of the value. </typeparam>
    /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
    /// <returns> A <see cref="Some{TSource}"/> containing the value. </returns>
    public static Some<TSource> Some<TSource>([DisallowNull] this TSource value) => new(value);

    /// <summary>
    /// Gets the singleton instance of <see cref="None{TSource}"/>.
    /// </summary>
    /// <param name="_"> Arbitrary value of type <typeparamref name="TSource"/>. </param>
    /// <typeparam name="TSource"> The type of the value. </typeparam>
    /// <returns> The singleton instance of <see cref="None{TSource}"/>. </returns>
    public static None<TSource> None<TSource>(this TSource? _) => Common.None<TSource>.Instance;

    /// <summary>
    /// Creates a <see cref="Some{TSource}"/> if the given value is not null.
    /// Gets the singleton instance of <see cref="None{TSource}"/> otherwise.
    /// </summary>
    /// <param name="value"> The value to wrap. </param>
    /// <typeparam name="TSource"> The type of the value. </typeparam>
    /// <returns> A <see cref="Some{TSource}"/> containing the value if it is not null, otherwise the singleton instance of <see cref="None{TSource}"/>. </returns>
    public static Maybe<TSource> Maybe<TSource>(this TSource? value) => value is null ? value.None() : value.Some();

    /// <summary>
    /// Extracts the value from a <see cref="Some{TSource}"/>.
    /// Otherwise returns the given default value or the type's default value.
    /// </summary>
    /// <param name="maybe"> The maybe to extract the value from. </param>
    /// <param name="defaultValue"> The default value to return if the maybe is not a <see cref="Some{TSource}"/>. If not specified, the type's default value is used. </param>
    /// <typeparam name="TSource"> The type of the value. </typeparam>
    /// <returns> The value if the maybe is a <see cref="Some{TSource}"/>, otherwise the given default value or the type's default value. </returns>
    public static TSource? TryGetValue<TSource>(this Maybe<TSource> maybe, TSource? defaultValue = default) =>
        maybe switch
        {
            Some<TSource> some => some.Value,
            _                  => defaultValue,
        };

    /// <summary>
    /// Runs the given function if the maybe is a <see cref="Some{TSource}"/>.
    /// </summary>
    /// <param name="maybe"> The maybe to run the function on. </param>
    /// <param name="func"> The function to run. </param>
    /// <typeparam name="TSource"> The type of the value. </typeparam>
    /// <typeparam name="TResult"> The type of the result. </typeparam>
    /// <returns> If <paramref name="maybe"/> is <see cref="Some{TSource}"/> the result of the given function, else <see cref="None{TSource}"/>. </returns>
    public static Maybe<TResult>
        Bind<TSource, TResult>(this Maybe<TSource> maybe, Func<TSource, Maybe<TResult>> func) =>
        maybe switch
        {
            Some<TSource> some => func(some.Value),
            _                  => Common.None<TResult>.Instance,
        };

    /// <summary>
    /// Runs the given function if the maybe is a <see cref="Some{TSource}"/>.
    /// </summary>
    /// <param name="maybe"> The maybe to run the function on. </param>
    /// <param name="onSome"> The function to run if the maybe is a <see cref="Some{TSource}"/>. </param>
    /// <param name="onNone"> The function to run if the maybe is a <see cref="None{TSource}"/>. </param>
    /// <typeparam name="TSource"> The type of the value. </typeparam>
    /// <typeparam name="TResult"> The type of the result. </typeparam>
    /// <returns> If <paramref name="maybe"/> is <see cref="Some{TSource}"/> the result of the <paramref name="onSome"/>, else the result of <paramref name="onNone"/>. </returns>
    public static Maybe<TResult> Bind<TSource, TResult>(this Maybe<TSource>           maybe,
                                                        Func<TSource, Maybe<TResult>> onSome,
                                                        Func<Maybe<TResult>>          onNone) =>
        maybe switch
        {
            Some<TSource> some => onSome(some.Value),
            _                  => onNone(),
        };
}