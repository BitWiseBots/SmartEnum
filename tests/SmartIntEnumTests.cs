using FluentAssertions;

namespace BitWiseBots.SmartEnum.Tests
{
    public class SmartIntEnumTests
    {
        [Fact]
        public void SmartIntEnum_ShouldImplicitlyCovertToTBase()
        {
            var testEnum = TestIntEnum.First;
            int value = testEnum;
            value.Should().Be(1);
        }

        public class TestIntEnum : SmartIntEnum<TestIntEnum>
        {
            public static readonly TestIntEnum First = new(nameof(First), 1);
            public static readonly TestIntEnum Second = new(nameof(Second), 2);

            private TestIntEnum(string name, int value) : base(name, value)
            {
            }
        }
    }
}