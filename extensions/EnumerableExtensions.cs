namespace Common.Extensions;

public static class EnumerableExtensions
{
    public static void ForEach<TSource>(this IEnumerable<TSource> collection, Action<TSource> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> source) => source ?? Enumerable.Empty<TSource>();

    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
    {
        if (source == null) throw new ArgumentNullException(nameof(source), $"The provided argument {nameof(source)} must not be Null.");

        if (batchSize <= 0) throw new ArgumentOutOfRangeException(nameof(batchSize));

        T[] batch = null;
        var elementIndex = 0;

        foreach (var item in source)
        {
            // fill batch
            batch ??= new T[batchSize];
            batch[elementIndex++] = item;
            if (elementIndex != batchSize) continue;
            
            // yield return full batch
            yield return batch;
            batch = null;
            elementIndex = 0;
        }

        // yield return partial batch
        if (batch != null && elementIndex > 0)
            yield return batch.Take(elementIndex);
    }    
}
