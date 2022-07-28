namespace Common.Extensions;

/// <summary>
/// Extends class instances with useful methods.
/// </summary>
public static class ClassExtensions
{
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