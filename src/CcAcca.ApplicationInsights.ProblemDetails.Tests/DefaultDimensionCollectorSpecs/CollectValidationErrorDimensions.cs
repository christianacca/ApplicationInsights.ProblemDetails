using System.Collections.Generic;
using CcAcca.ApplicationInsights.ProblemDetails;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Specs.DefaultDimensionCollectorSpecs
{
    public class CollectValidationErrorDimensions
    {
        private static ProblemDetailsTelemetryOptions DefaultOptions => TestFixture.DefaultOptions;

        [Fact]
        public void Empty_errors()
        {
            // given
            var sut = Sut();
            var problem = new ValidationProblemDetails();

            // when
            var d = new Dictionary<string, string>();
            sut.CollectValidationErrorDimensions(d, problem, null);

            // then
            d.Should().BeEmpty();
        }

        [Fact]
        public void One_error_key_one_message()
        {
            // given
            var sut = Sut();
            var errors = new Dictionary<string, string[]> { { "prop1", new[] { "value1" } } };
            var problem = new ValidationProblemDetails(errors);

            // when
            var d = new Dictionary<string, string>();
            sut.CollectValidationErrorDimensions(d, problem, null);

            // then
            var expected = new Dictionary<string, string>
            {
                { $"{DefaultOptions.DimensionPrefix}.Errors.prop1", "value1" }
            };
            d.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void One_error_key_two_messages()
        {
            // given
            var sut = Sut();
            var errors = new Dictionary<string, string[]> { { "prop1", new[] { "value1", "value2" } } };
            var problem = new ValidationProblemDetails(errors);

            // when
            var d = new Dictionary<string, string>();
            sut.CollectValidationErrorDimensions(d, problem, null);

            // then
            var expected = new Dictionary<string, string>
            {
                { $"{DefaultOptions.DimensionPrefix}.Errors.prop1", "value1; value2" }
            };
            d.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void One_error_empty_key_two_messages()
        {
            // given
            var sut = Sut();
            var errors = new Dictionary<string, string[]> { { "", new[] { "value1", "value2" } } };
            var problem = new ValidationProblemDetails(errors);

            // when
            var d = new Dictionary<string, string>();
            sut.CollectValidationErrorDimensions(d, problem, null);

            // then
            var expected = new Dictionary<string, string>
            {
                { $"{DefaultOptions.DimensionPrefix}.Errors", "value1; value2" }
            };
            d.Should().BeEquivalentTo(expected);
        }

        private static TestDefaultDimensionCollector Sut()
        {
            return new TestDefaultDimensionCollector(TestFixture.OptionsOf(TestFixture.DefaultOptions));
        }
    }
}