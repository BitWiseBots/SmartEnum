namespace BitWiseBots.SmartEnum
{
    /// <summary>
    /// Provides a base class for implementing Smart Enums with an underlying int type.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum itself.</typeparam>
    public abstract class SmartIntEnum<TEnum>(string name, int value) : SmartEnum<TEnum, int>(name, value)
        where TEnum : SmartIntEnum<TEnum>;
}