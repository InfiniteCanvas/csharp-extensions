using System.Diagnostics.CodeAnalysis;

namespace Common;

public static class MaybeExtensions
{
    public static Some<TSource> Some<TSource>([DisallowNull] this TSource value) => new(value);

    public static None<TSource> None<TSource>(this TSource? _) => Common.None<TSource>.Instance;

    public static Maybe<TSource> Maybe<TSource>(this TSource? value) => value is null ? value.None() : value.Some();

    public static TSource? TryGetValue<TSource>(this Maybe<TSource> maybe, TSource? defaultValue = default) =>
        maybe switch
        {
            Some<TSource> some => some.Value,
            _                  => defaultValue,
        };

    public static TResult? Map<TSource, TResult>(this Maybe<TSource>    maybe,
                                                 Func<TSource, TResult> map,
                                                 TResult?               defaultValue = default) =>
        maybe switch
        {
            Some<TSource> some => map(some.Value),
            _                  => defaultValue,
        };

    public static Maybe<TResult> Bind<TSource, TResult>(this Maybe<TSource>           maybe,
                                                        Func<TSource, Maybe<TResult>> bind,
                                                        TResult?                      defaultValue = default) =>
        maybe switch
        {
            Some<TSource> some => bind(some.Value),
            _                  => defaultValue.None(),
        };
}