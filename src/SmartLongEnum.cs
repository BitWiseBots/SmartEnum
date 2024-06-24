namespace BitWiseBots.SmartEnum
{
    /// <summary>
    /// Provides a base class for implementing Smart Enums with an underlying long type.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum itself.</typeparam>
    public abstract class SmartLongEnum<TEnum>(string name, long value) : SmartEnum<TEnum, long>(name, value)
        where TEnum : SmartLongEnum<TEnum>;
}