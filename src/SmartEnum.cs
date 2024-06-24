using System.Diagnostics.CodeAnalysis;

namespace BitWiseBots.SmartEnum
{
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    /// Provides a base class for implementing the Smart Enum pattern, with the ability to specify any struct type for the underlying value.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum itself, used in a self-referential pattern to ensure type-safe operations within the SmartEnum class. The constraint, TEnum : SmartEnum&lt;TEnum, TBase&gt;, mandates that TEnum inherits from SmartEnum, facilitating static polymorphism for accessing static members and methods in a type-safe manner.</typeparam>
    /// <typeparam name="TBase">The underlying value type of the enum.</typeparam>
    /// <param name="value">The value of the enum item.</param>
    /// <param name="name">The name of the enum item.</param>
    [SuppressMessage("Sonar Code Smell", "S4035",
        Justification = """
                        This class implements the Smart Enum pattern, where equality is fundamentally based on the enum's identity (value and name) rather than mutable state. 
                        Derived types are intended to extend functionality without altering this core identity concept. 
                        Therefore, equality checks are designed to be consistent across all instances and derived types, focusing solely on the base class's properties. 
                        This design choice ensures that the Smart Enum acts as a value type, akin to traditional enums, 
                        where equality is determined by the enum's value rather than its reference or the state of any derived class
                        """)]
    public abstract class SmartEnum<TEnum, TBase>(string name, TBase value) : IEquatable<SmartEnum<TEnum, TBase>>
        where TEnum : SmartEnum<TEnum, TBase>
        where TBase : struct
    {
        /// <summary>
        /// Initializes the dictionaries to store enum instances by value and by name.
        /// </summary>
        private static readonly (Dictionary<TBase, TEnum> ByValue, Dictionary<string, TEnum> ByName) All = InitializeAll();

        /// <summary>
        /// Initializes the <see cref="All"/> static member, populating it with enum instances.
        /// </summary>
        /// <returns>A tuple containing two dictionaries: one mapping values to enum instances, and another mapping names to enum instances.</returns>
        private static (Dictionary<TBase, TEnum> ByValue, Dictionary<string, TEnum> ByName) InitializeAll()
        {
            var type = typeof(TEnum);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => type.IsAssignableFrom(fi.FieldType))
                .Select(fi => (TEnum)fi.GetValue(null)!)
                .ToList();

            var byValue = fields.ToDictionary(e => e.Value);
            var byName = fields.ToDictionary(e => e.Name);

            return (byValue, byName);
        }

        /// <summary>
        /// Gets the dictionary mapping values to enum instances.
        /// </summary>
        private static Dictionary<TBase, TEnum> AllByValue => All.ByValue;

        /// <summary>
        /// Gets the dictionary mapping names to enum instances.
        /// </summary>
        private static Dictionary<string, TEnum> AllByName => All.ByName;

        /// <summary>
        /// Gets the value of the enum item.
        /// </summary>
        public TBase Value { get; } = value;

        /// <summary>
        /// Gets the name of the enum item.
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        /// Implicitly converts a SmartEnum instance to its underlying base value.
        /// This allows a SmartEnum instance to be used in contexts where its base value is expected, 
        /// providing seamless integration with the underlying value type.
        /// </summary>
        /// <param name="smartEnum">The SmartEnum instance to convert.</param>
        /// <returns>The underlying base value of the SmartEnum instance.</returns>
        public static implicit operator TBase(SmartEnum<TEnum, TBase> smartEnum)
        {
            return smartEnum.Value;
        }

        /// <summary>
        /// Explicitly converts a base value to a SmartEnum instance.
        /// This conversion requires explicit casting in code to signal the intention of converting 
        /// from the base value to a SmartEnum instance, ensuring that such conversions are made consciously 
        /// by the developer to avoid unintentional conversions.
        /// </summary>
        /// <param name="value">The base value to convert to a SmartEnum instance.</param>
        /// <returns>A SmartEnum instance corresponding to the given base value.</returns>
        /// <exception cref="InvalidCastException">Thrown when the base value does not correspond to any SmartEnum instance.</exception>
        public static explicit operator SmartEnum<TEnum, TBase>(TBase value)
        {
            return FromValue(value) ?? throw new InvalidCastException();
        }

        /// <summary>
        /// Determines if two SmartEnum instances are equal based on their values.
        /// </summary>
        /// <param name="left">The left SmartEnum instance.</param>
        /// <param name="right">The right SmartEnum instance.</param>
        /// <returns>True if the instances are equal; otherwise, false.</returns>
        public static bool operator ==(SmartEnum<TEnum, TBase> left, SmartEnum<TEnum, TBase> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines if two SmartEnum instances are not equal based on their values.
        /// </summary>
        /// <param name="left">The left SmartEnum instance.</param>
        /// <param name="right">The right SmartEnum instance.</param>
        /// <returns>True if the instances are not equal; otherwise, false.</returns>
        public static bool operator !=(SmartEnum<TEnum, TBase> left, SmartEnum<TEnum, TBase> right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Retrieves a SmartEnum instance by its value.
        /// </summary>
        /// <param name="value">The value of the SmartEnum instance to retrieve.</param>
        /// <returns>The SmartEnum instance if found; otherwise, null.</returns>
        [PublicAPI]
        public static TEnum? FromValue(TBase value)
        {
            #if NETSTANDARD2_0
            return AllByValue.TryGetValue(value, out var result) ? result : default;
            #elif NETSTANDARD2_1
            return AllByValue.GetValueOrDefault(value);
            #endif
        }

        /// <summary>
        /// Retrieves a SmartEnum instance by its name.
        /// </summary>
        /// <param name="name">The name of the SmartEnum instance to retrieve.</param>
        /// <returns>The SmartEnum instance if found; otherwise, null.</returns>
        [PublicAPI]
        public static TEnum? FromName(string name)
        {
            return AllByName.Values.SingleOrDefault(e => e.Name == name);
        }

        /// <summary>
        /// Retrieves all SmartEnum instances.
        /// </summary>
        /// <returns>An IEnumerable of all SmartEnum instances.</returns>
        [PublicAPI]
        public static IEnumerable<TEnum> GetAll()
        {
            return AllByValue.Values;
        }

        /// <summary>
        /// Determines if the current SmartEnum instance is equal to another SmartEnum instance.
        /// </summary>
        /// <param name="other">The other SmartEnum instance to compare with.</param>
        /// <returns>True if the instances are equal; otherwise, false.</returns>
        public bool Equals(SmartEnum<TEnum, TBase>? other)
        {
            return other is not null && Value.Equals(other.Value);
        }

        /// <summary>
        /// Determines if the current SmartEnum instance is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if the object is a SmartEnum instance and is equal to the current instance; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return obj is SmartEnum<TEnum, TBase> other && Equals(other);
        }

        /// <summary>
        /// Gets the hash code for the SmartEnum instance.
        /// </summary>
        /// <returns>A hash code for the current SmartEnum instance.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Returns the name of the SmartEnum instance.
        /// </summary>
        /// <remarks>
        /// This method overrides the <see cref="Object.ToString"/> method to provide a more meaningful string representation of the SmartEnum instance. 
        /// Instead of returning the type name or a default representation, it returns the <see cref="Name"/> property of the SmartEnum, 
        /// which typically represents the identifier or label of the enum entry. This makes it easier to display or log the SmartEnum instance in a human-readable format.
        /// </remarks>
        /// <returns>A <see cref="string"/> that represents the name of the SmartEnum instance.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}