using CcAcca.ApplicationInsights.ProblemDetails;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Specs.DefaultDimensionCollectorSpecs
{
    public class SerializeValue
    {
        [Fact]
        public void String_value()
        {
            Serialize("value").Should().Be("value");
        }

        [Fact]
        public void Int32_value()
        {
            Serialize(10).Should().Be("10");
        }

        [Fact]
        public void Nullable_Int32_value()
        {
            int? value = 10;
            Serialize(value).Should().Be("10");
        }

        [Fact]
        public void Nullable_Int32_null_value()
        {
            int? value = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            Serialize(value).Should().BeNull();
        }

        [Fact]
        public void Int64_value()
        {
            const long value = 10;
            Serialize(value).Should().Be("10");
        }

        [Fact]
        public void DateTime_default_kind_value()
        {
            var dtm = new DateTime(1999, 12, 31, 23, 59, 59);
            Serialize(dtm).Should().Be("1999-12-31T23:59:59.0000000");
        }

        [Fact]
        public void DateTime_utc_value()
        {
            var dtm = new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            Serialize(dtm).Should().Be("1999-12-31T23:59:59.0000000Z");
        }

        [Fact]
        public void DateTimeOffset_value()
        {
            var dtm = new DateTimeOffset(new DateTime(1999, 12, 31, 23, 59, 59));
            Serialize(dtm).Should().Be("1999-12-31T23:59:59.0000000+00:00");
        }
        
        [Fact]
        public void DateOnly_value()
        {
            var d = new DateOnly(2023, 09, 12);
            Serialize(d).Should().Be("2023-09-12");
        }

        [Fact]
        public void TimeOnly_value()
        {
            var t = new TimeOnly(14, 04, 12);
            Serialize(t).Should().Be("14:04:12.0000000");
        }

        [Fact]
        public void Dictionary_value()
        {
            var value = new Dictionary<string, int>
            {
                { "key1", 1 },
                { "key2", 2 }
            };
            const string expected = "{\"key1\":1,\"key2\":2}";
            Serialize(value).Should().Be(expected);
        }

        [Fact]
        public void Custom_object_value()
        {
            var value = new CustomObject
            {
                Prop1 = "one",
                Prop2 = 2
            };
            const string expected = "{\"Prop1\":\"one\",\"Prop2\":2}";
            Serialize(value).Should().Be(expected);
        }

        [Fact]
        public void Problem_Details_With_Extensions()
        {
            var value = new ProblemDetails
            {
                Extensions =
                {
                    { "Prop1", "one" },
                    { "Prop2", 2 }
                }
            };
            const string expected = "{\"Prop1\":\"one\",\"Prop2\":2}";
            Serialize(value).Should().Be(expected);
        }

        [Fact]
        public void Custom_Problem_Details_With_Extensions()
        {
            var value = new CustomProblemDetails
            {
                Prop1 = "one",
                Prop2 = 2,
                Extensions =
                {
                    { "Prop3", "three" },
                    { "Prop4", 4 }
                }
            };
            const string expected = "{\"Prop1\":\"one\",\"Prop2\":2,\"Prop3\":\"three\",\"Prop4\":4}";
            Serialize(value).Should().Be(expected);
        }

        [Fact]
        public void Custom_struct_value()
        {
            var value = new CustomStruct
            {
                Prop1 = "one",
                Prop2 = 2
            };
            const string expected = "{\"Prop1\":\"one\",\"Prop2\":2}";
            Serialize(value).Should().Be(expected);
        }

        [Fact]
        public void On_json_serialization_exception_should_return_null()
        {
            var value = new CircularCustomObject
            {
                Prop1 = "one"
            };
            value.Self = value;

            Serialize(value).Should().BeNull();
        }

        private static string? Serialize(object? value)
        {
            return DefaultDimensionCollector.SerializeValue(null!, null!, "key", value);
        }


        public class CustomObject
        {
            public string? Prop1 { get; set; }
            public int Prop2 { get; set; }
        }

        public class CustomProblemDetails : ProblemDetails
        {
            public string? Prop1 { get; set; }
            public int Prop2 { get; set; }
        }

        public class CircularCustomObject
        {
            public string? Prop1 { get; set; }

            public CircularCustomObject? Self { get; set; }
        }

        public struct CustomStruct
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
        }
    }
}