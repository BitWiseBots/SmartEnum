using FluentAssertions;

namespace BitWiseBots.SmartEnum.Tests
{
    public class SmartLongEnumTests
    {
        [Fact]
        public void SmartLongEnum_ShouldImplicitlyCovertToTBase()
        {
            var testEnum = TestLongEnum.First;
            long value = testEnum;
            value.Should().Be(1);
        }

        public class TestLongEnum : SmartLongEnum<TestLongEnum>
        {
            public static readonly TestLongEnum First = new(nameof(First), 1);
            public static readonly TestLongEnum Second = new(nameof(Second), 2);

            private TestLongEnum(string name, long value) : base(name, value)
            {
            }
        }
    }
}