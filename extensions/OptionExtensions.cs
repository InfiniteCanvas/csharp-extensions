namespace Common.Extensions;

/// <summary>
///     Methods for Option for lazy bums like me
/// </summary>
public static class OptionExt
{

    /// <summary>
    ///     Make Some Option
    /// </summary>
    /// <param name="value">Value to wrap</param>
    /// <typeparam name="TValue">Wrapped type</typeparam>
    /// <returns>Option with Some</returns>
    public static Option<TValue> Some<TValue>(TValue value) =>
        new Some<TValue>(value);

    /// <summary>
    ///     Make None Option
    /// </summary>
    /// <typeparam name="TValue">Wrapped type</typeparam>
    /// <returns>Option with None</returns>
    public static Option<TValue> None<TValue>() =>
        new None<TValue>();

    /// <summary>
    ///     Creates an optional from a nullable, returns None Option if null, else Some Option
    /// </summary>
    /// <param name="value">Value to wrap</param>
    /// <typeparam name="TValue">Wrapped Type</typeparam>
    /// <returns>Option with Some or None</returns>
    public static Option<TValue> FromNullable<TValue>(TValue value) where TValue : new() =>
        value != null ? Some(value) : None<TValue>();

    /// <summary>
    ///     Creates an optional from a nullable, returns None Option if null, else Some Option
    /// </summary>
    /// <param name="value">Value to wrap</param>
    /// <typeparam name="TValue">Wrapped Type</typeparam>
    /// <returns>Option with Some or None</returns>
    public static Option<TValue> FromNullable<TValue>(TValue? value) where TValue : struct =>
        value != null ? Some(value.Value) : None<TValue>();

    /// <summary>
    ///     Creates an Option for functions that might fail using try/catch
    ///     Returns Some if it succeeds,
    ///     None if it fails
    /// </summary>
    /// <param name="function">The function</param>
    /// <typeparam name="TValue">Wrapped type</typeparam>
    /// <returns>Some or None Option</returns>
    public static Option<TValue> TryCatch<TValue>(Func<TValue> function)
    {
        try
        {
            return Some(function());
        }
        catch
        {
            return None<TValue>();
        }
    }

}