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

    public static Option<TResult> Map<TInput, TResult>(this Option<TInput> @this, Func<TInput, TResult> func) => @this.TryCatch(func);
    public static Option<TResult> Bind<TInput, TResult>(this Option<TInput> @this, Func<TInput, Option<TResult>> func) => @this.TryCatch(func);

}