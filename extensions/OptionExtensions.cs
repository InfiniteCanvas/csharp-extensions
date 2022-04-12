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
    public static Option<TValue> Some<TValue>(this TValue value) => new Some<TValue>(value);

    /// <summary>
    ///     Make None Option
    /// </summary>
    /// <typeparam name="TValue">Wrapped type</typeparam>
    /// <returns>Option with None</returns>
    public static Option<TValue> None<TValue>(this TValue _) => new None<TValue>();

    /// <summary>
    ///     Creates an optional from a nullable, returns None Option if null, else Some Option
    /// </summary>
    /// <param name="value">Value to wrap</param>
    /// <typeparam name="TValue">Wrapped Type</typeparam>
    /// <returns>Option with Some or None</returns>
    public static Option<TValue> MaybeNull<TValue>(this TValue value) where TValue : new() =>
        value != null ? value.Some() : value.None();

    /// <summary>
    ///     Creates an optional from a nullable, returns None Option if null, else Some Option
    /// </summary>
    /// <param name="value">Value to wrap</param>
    /// <typeparam name="TValue">Wrapped Type</typeparam>
    /// <returns>Option with Some or None</returns>
    public static Option<TValue> MaybeNull<TValue>(this TValue? value) where TValue : struct =>
        value != null ? Some(value.Value) : new None<TValue>();


    /// <summary>
    ///     Creates an Option for functions that might fail using try/catch
    ///     Returns Some if it succeeds,
    ///     None if it fails
    /// </summary>
    /// <typeparam name="TValue">Wrapped type</typeparam>
    /// <param name="func">The function</param>
    /// <returns>Some or None</returns>
    public static Option<TValue> TryCatch<TValue>(this Func<TValue> func)
    {
        try { return Some(func()); }
        catch { return new None<TValue>(); }
    }

    public static Option<TResult> TryCatch<TValue, TResult>(this Option<TValue> @this, Func<TValue, TResult> func)
    {
        try { return Some(func(@this)); }
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
    ///     None on faiure
    /// </returns>
    public static Option<TResult> TryMap<TInput, TResult>(this Option<TInput> @this, Func<TInput, TResult> func) => @this.TryCatch(func);

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
    ///     None on faiure
    /// </returns>
    public static Option<TResult> TryBind<TInput, TResult>(this Option<TInput> @this, Func<TInput, Option<TResult>> func) => @this.TryCatch(func);

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
    public static Option<TResult> MaybeDo<TInput, TResult>(this Option<TInput> @this, Func<TInput, TResult> @do, Func<TInput, TResult> dont) => @this switch
    {
        Some<TInput> some => @do(some),
        None<TInput> none => dont(none),
        _ => throw new Exception("Shouldn't happen")
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
        switch (@this)
        {
            case Some<TInput> some: @do(some); break;
            case None<TInput> _: dont(); break;
        };
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
    public static Option<TResult> MaybeDoBind<TInput, TResult>(this Option<TInput> @this, Func<TInput, Option<TResult>> @do, Func<TInput, Option<TResult>> dont) => @this switch
    {
        Some<TInput> some => @do(some),
        None<TInput> none => dont(none),
        _ => throw new Exception("Shouldn't happen")
    };
}