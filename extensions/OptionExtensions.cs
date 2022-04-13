using System.Diagnostics.Contracts;

namespace Common.Extensions;

/// <summary>
///     Methods for Option for lazy bums like me
/// </summary>
public static class OptionExt
{
    /// <summary>
    ///     Make Some Option
    /// </summary>
    /// <typeparam name="TValue">Wrapped type</typeparam>
    /// <param name="value">Value to wrap</param>
    /// <returns>Some Option</returns>
    public static Option<TValue> Some<TValue>(this TValue value) =>
        value switch
        {
            not null => new Some<TValue>(value),
            _        => new None<TValue>()
        };

    /// <summary>
    ///     Make None Option
    /// </summary>
    /// <typeparam name="TValue">Wrapped type</typeparam>
    /// <returns>Option with None</returns>
    public static Option<TValue> None<TValue>(this TValue _) => new None<TValue>();

    public static Option<TValue> FromMaybeNull<TValue>(this TValue value) =>
        value switch
        {
            not null => value.Some(),
            _        => new None<TValue>()
        };

    public static TValue TryGet<TValue>(this Option<TValue> @this) =>
        @this switch
        {
            Some<TValue> some => some,
            _                 => throw new Exception()
        };

    /// <summary>
    ///     Creates an Option for functions that might fail using try/catch
    ///     Returns Some if it succeeds,
    ///     None if it fails
    /// </summary>
    /// <typeparam name="TResult">Wrapped type</typeparam>
    /// <param name="func">The function</param>
    /// <returns>Some or None</returns>
    public static Option<TResult> TryCatch<TResult>(this Func<TResult> func)
    {
        try { return Some(func()); }
        catch { return new None<TResult>(); }
    }

    public static Option<TResult> TryCatch<TInput, TResult>(this Option<TInput> @this, Func<TInput, TResult> func)
    {
        try { return Some(func(@this.TryGet())); }
        catch { return new None<TResult>(); }
    }

    public static Option<TResult> TryCatch<TInput, TResult>(this Option<TInput>           @this,
                                                            Func<TInput, Option<TResult>> func)
    {
        try { return func(@this.TryGet()); }
        catch { return new None<TResult>(); }
    }

    /// <summary>
    ///     Tries to map the Option from one type to another.
    ///     Returns Some on success, None on failure.
    /// </summary>
    /// <typeparam name="TInput">Input Type</typeparam>
    /// <typeparam name="TResult">Output Type</typeparam>
    /// <param name="this">Option instance</param>
    /// <param name="func">Mapping function</param>
    /// <returns>
    ///     Some on success
    ///     None on failure
    /// </returns>
    public static Option<TResult> TryMap<TInput, TResult>(this Option<TInput> @this, Func<TInput, TResult> func) =>
        @this.TryCatch(func);

    /// <summary>
    ///     Tries to bind the Option from one type to an Option.
    ///     Returns Some on success, None on failure.
    /// </summary>
    /// <typeparam name="TInput">Input Type</typeparam>
    /// <typeparam name="TResult">Output Type</typeparam>
    /// <param name="this">Option instance</param>
    /// <param name="func">Binding function</param>
    /// <returns>
    ///     Some on success
    ///     None on failure
    /// </returns>
    public static Option<TResult> TryBind<TInput, TResult>(this Option<TInput>           @this,
                                                           Func<TInput, Option<TResult>> func) =>
        @this.TryCatch(func);

    /// <summary>
    ///     Checks if Option is Some
    /// </summary>
    /// <typeparam name="TValue">Wrapped type</typeparam>
    /// <param name="this">Option to check</param>
    /// <returns>
    ///     True if Some
    ///     False if None
    /// </returns>
    public static bool IsSome<TValue>(this Option<TValue> @this) => @this is Some<TValue>;

    /// <summary>
    ///     Checks if Option is None
    /// </summary>
    /// <typeparam name="TValue">Wrapped type</typeparam>
    /// <param name="this">Option to check</param>
    /// <returns>
    ///     True if None
    ///     False if Some
    /// </returns>
    public static bool IsNone<TValue>(this Option<TValue> @this) => @this is None<TValue>;

    /// <summary>
    ///     Does stuff if Some using the Some value
    ///     Does something else if None using the None value
    /// </summary>
    /// <typeparam name="TInput">Wrapped type</typeparam>
    /// <typeparam name="TResult">Output type of functions</typeparam>
    /// <param name="this">Input Option</param>
    /// <param name="do">Function run on Some.Value</param>
    /// <param name="dont">Function run on None</param>
    /// <returns>Result of the function run</returns>
    /// <exception cref="Exception">Shouldn't happen</exception>
    public static Option<TResult> MaybeMap<TInput, TResult>(this Option<TInput>   @this,
                                                            Func<TInput, TResult> @do,
                                                            Func<TResult>         dont) =>
        @this switch
        {
            Some<TInput> some => @do(some).FromMaybeNull(),
            None<TInput> _    => dont().None(),
            _                 => throw new ArgumentOutOfRangeException(nameof(@this), @this, null)
        };

    /// <summary>
    ///     Does stuff if Some using Some value
    ///     Does something else if None
    /// </summary>
    /// <typeparam name="TInput">Wrapped type</typeparam>
    /// <param name="this">Input Option</param>
    /// <param name="do">Function run on Some.Value</param>
    /// <param name="dont">Function run if None</param>
    public static void MaybeDo<TInput>(this Option<TInput> @this, Action<TInput> @do, Action dont)
    {
        if (@this is Some<TInput> some) @do(some);
        else dont();
    }

    /// <summary>
    ///     Does stuff if Some using the Some value
    ///     Does something else if None using the None value
    /// </summary>
    /// <typeparam name="TInput">Wrapped input type</typeparam>
    /// <typeparam name="TResult">Wrapped output type</typeparam>
    /// <param name="this">Input Option</param>
    /// <param name="do">Function run on Some.Value</param>
    /// <param name="dont">Function run if None</param>
    /// <returns>Option output</returns>
    /// <exception cref="Exception"></exception>
    public static Option<TResult> MaybeDoBind<TInput, TResult>(this Option<TInput>           @this,
                                                               Func<TInput, Option<TResult>> @do,
                                                               Func<None<TResult>>           dont) =>
        @this switch
        {
            Some<TInput> some => @do(some),
            None<TInput> _    => dont(),
            _                 => throw new Exception("Shouldn't happen")
        };
}