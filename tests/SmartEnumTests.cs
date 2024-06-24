using FluentAssertions;

namespace BitWiseBots.SmartEnum.Tests
{
    public class SmartEnumTests
    {
        [Fact]
        public void SmartEnum_ShouldImplicitlyCovertToTBase()
        { 
            var testEnum = TestEnum.First;
            int value = testEnum;
            value.Should().Be(1);
        }

        [Fact]
        public void SmartEnum_ShouldExplicitlyConvertFromTBase()
        {
            var testEnum = (TestEnum)2;
            testEnum.Should().Be(TestEnum.Second);
        }

        [Fact]
        public void EqualsOperator_ShouldReturnTrue_WhenBothValuesAreEqual()
        {
            var first = TestEnum.First;
            var second = TestEnum.First;
            (first == second).Should().BeTrue();
        }

        [Fact]
        public void EqualsOperator_ShouldReturnFalse_WhenValuesAreNotEqual()
        {
            var first = TestEnum.First;
            var second = TestEnum.Second;
            (first == second).Should().BeFalse();
        }

        [Fact]
        public void NotEqualsOperator_ShouldReturnTrue_WhenValuesAreNotEqual()
        {
            var first = TestEnum.First;
            var second = TestEnum.Second;
            (first != second).Should().BeTrue();
        }

        [Fact]
        public void NotEqualsOperator_ShouldReturnFalse_WhenValuesAreEqual()
        {
            var first = TestEnum.First;
            var second = TestEnum.First;
            (first != second).Should().BeFalse();
        }

        [Fact]
        public void EqualsSmartEnum_ShouldReturnTrue_WhenValuesAreEqual()
        {
            var first = TestEnum.First;
            var second = TestEnum.First;
            first.Equals(second).Should().BeTrue();
        }

        [Fact]
        public void EqualsSmartEnum_ShouldReturnFalse_WhenValuesAreNotEqual()
        {
            var first = TestEnum.First;
            var second = TestEnum.Second;
            first.Equals(second).Should().BeFalse();
        }

        [Fact]
        public void EqualsSmartEnum_ShouldReturnFalse_WhenOtherIsNull()
        {
            var first = TestEnum.First;
            first.Equals(null).Should().BeFalse();
        }
        
        [Fact]
        public void EqualsObject_ShouldReturnTrue_WhenValuesAreEqual()
        {
            var first = TestEnum.First;
            var second = TestEnum.First;
            first.Equals((object)second).Should().BeTrue();
        }

        [Fact]
        public void EqualsObject_ShouldReturnFalse_WhenValuesAreNotEqual()
        {
            var first = TestEnum.First;
            var second = TestEnum.Second;
            first.Equals((object)second).Should().BeFalse();
        }

        [Fact]
        public void EqualsObject_ShouldReturnFalse_WhenOtherIsNull()
        {
            var first = TestEnum.First;
            first.Equals((object?)null).Should().BeFalse();
        }

        [Fact]
        public void EqualsObject_ShouldReturnFalse_WhenOtherIsDifferentType()
        {
            var first = TestEnum.First;
            var second = TestEnum2.First;
            // ReSharper disable once SuspiciousTypeConversion.Global
            first.Equals(second).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameValue_WhenValuesAreEqual()
        {
            var first = TestEnum.First;
            var second = TestEnum.First;
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentValue_WhenValuesAreNotEqual()
        {
            var first = TestEnum.First;
            var second = TestEnum.Second;
            first.GetHashCode().Should().NotBe(second.GetHashCode());
        }

        [Fact]
        public void FromValue_ShouldReturnSmartEnum_WhenValueExists()
        {
            var testEnum = TestEnum.FromValue(1);
            testEnum.Should().Be(TestEnum.First);
        }

        [Fact]
        public void FromValue_ShouldReturnNull_WhenValueDoesNotExist()
        {
            var testEnum = TestEnum.FromValue(3);
            testEnum.Should().BeNull();
        }

        [Fact]
        public void FromName_ShouldReturnSmartEnum_WhenNameExists()
        {
            var testEnum = TestEnum.FromName("First");
            testEnum.Should().Be(TestEnum.First);
        }

        [Fact]
        public void FromName_ShouldReturnNull_WhenNameDoesNotExist()
        {
            var testEnum = TestEnum.FromName("Third");
            testEnum.Should().BeNull();
        }

        [Fact]
        public void GetAll_ShouldReturnAllSmartEnums()
        {
            var testEnums = TestEnum.GetAll();
            testEnums.Should().BeEquivalentTo(new[] { TestEnum.First, TestEnum.Second });
        }

        [Fact]
        public void ToString_ShouldReturnName()
        {
            var testEnum = TestEnum.First;
            testEnum.ToString().Should().Be("First");
        }

        public class TestEnum : SmartEnum<TestEnum, int>
        {
            public static readonly TestEnum First = new(nameof(First), 1);
            public static readonly TestEnum Second = new(nameof(Second), 2);

            private TestEnum(string name, int value) : base(name, value)
            {
            }
        }

        public class TestEnum2 : SmartEnum<TestEnum2, int>
        {
            public static readonly TestEnum2 First = new(nameof(First), 1);

            private TestEnum2(string name, int value) : base(name, value)
            {
            }
        }
    }
}