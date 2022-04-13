namespace Common.Extensions;

public static class ClassExtensions
{
    public static void TryCatch<TInput, TException>(this TInput @this, Action<TInput> @try, Action<TException> @catch)
        where TInput : class where TException : Exception
    {
        try { @try(@this); }
        catch (TException e) { @catch(e); }
    }

    public static Option<TResult> TryCatch<TInput, TResult, TException>(this TInput           @this,
                                                                        Func<TInput, TResult> @try,
                                                                        Action<TException>    @catch)
        where TInput : class where TException : Exception
    {
        try { return @try(@this).Some(); }
        catch (TException e)
        {
            @catch(e);
            return new None<TResult>();
        }
    }
}
