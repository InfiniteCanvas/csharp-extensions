namespace Extensions;

public static class ClassExtensions
{
    /// <summary>
    /// Executes given function with passed in value and returns its result, or <paramref name="defaultValue"/> if null.
    /// </summary>
    /// <param name="value">Value passed in.</param>
    /// <param name="map"> Function to map value to a new value.</param>
    /// <param name="defaultValue"> Default value to return if <paramref name="map"/> returns null.</param>
    /// <typeparam name="TSource">Type of source.</typeparam>
    /// <typeparam name="TResult">Type of result.</typeparam>
    /// <returns></returns>
    public static TResult? Bind<TSource, TResult>(this TSource           value,
                                                  Func<TSource, TResult> map,
                                                  TResult?               defaultValue = default) =>
        value.Bind(map) ?? defaultValue;

    /// <summary>
    /// Executes <paramref name="try"/> with passed in instance and catches any exceptions with <paramref name="catch"/>.
    /// </summary>
    /// <param name="instance"> Instance passed in. </param>
    /// <param name="try"> Function to try. </param>
    /// <param name="catch"> Function to catch exceptions. </param>
    /// <typeparam name="TSource"> Type of source passed in. </typeparam>
    /// <typeparam name="TException"> Type of exception to catch. </typeparam>
    public static void TryCatch<TSource, TException>(this TSource                instance,
                                                     Action<TSource>             @try,
                                                     Action<TSource, TException> @catch)
        where TException : Exception
    {
        try { @try(instance); }
        catch (TException e) { @catch(instance, e); }
    }
}