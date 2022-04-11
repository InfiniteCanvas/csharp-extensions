namespace Common.Extensions;

/// <summary>
///     Represents some value or none
/// </summary>
/// <typeparam name="TValue">The wrapped type</typeparam>
public abstract class Option<TValue>
{

    public override bool Equals(object obj)
    {
        switch (this)
        {
            case Some<TValue> some1 when obj is Some<TValue> some2:
                return Equals(some1.Value, some2.Value);
            case None<TValue> _ when obj is None<TValue> _:
                return true;
            default: return false;
        }
    }

    public override int GetHashCode()
    {
        switch (this)
        {
            case Some<TValue> some:
                return some.GetHashCode();
            case None<TValue> _:
                return 0;
            default:
                throw new Exception("Invalid type"); // Here for the compiler. Should never happen
        }
    }

    public static bool operator ==(Option<TValue> opt1, Option<TValue> opt2) =>
        Equals(opt1, opt2);

    public static bool operator !=(Option<TValue> opt1, Option<TValue> opt2) =>
        !Equals(opt1, opt2);

}

/// <summary>
///     Some value is there
/// </summary>
/// <typeparam name="TValue">The wrapped type</typeparam>
internal sealed class Some<TValue> : Option<TValue>
{

    public TValue Value { get; }


    public Some(TValue value) =>
        Value = value;

    public static implicit operator TValue(Some<TValue> value) => value.Value;
}

/// <summary>
///     No value is there
/// </summary>
/// <typeparam name="TValue">The wrapped type</typeparam>
internal sealed class None<TValue> : Option<TValue> { }