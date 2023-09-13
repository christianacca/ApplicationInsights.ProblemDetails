using CcAcca.ApplicationInsights.ProblemDetails;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Specs.DefaultDimensionCollectorSpecs
{
    public class CollectionStandardDimensions
    {
        private static ProblemDetailsTelemetryOptions DefaultOptions => TestFixture.DefaultOptions;

        [Fact]
        public void Default_problem()
        {
            // given
            var sut = Sut();
            var problem = new ProblemDetails();

            // when
            var d = new Dictionary<string, string>();
            sut.CollectionStandardDimensions(d, problem, null);

            // then
            var expected = new Dictionary<string, string>
            {
                { $"{DefaultOptions.DimensionPrefix}.Status", "0" }
            };
            d.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Complete_problem()
        {
            // given
            var sut = Sut();
            var problem = new ProblemDetails
            {
                Detail = "Some detail",
                Instance = "Some instance",
                Status = 400,
                Title = "Some title",
                Type = "Some type"
            };

            // when
            var d = new Dictionary<string, string>();
            sut.CollectionStandardDimensions(d, problem, null);

            // then
            // then
            var expected = new Dictionary<string, string>
            {
                { $"{DefaultOptions.DimensionPrefix}.Detail", "Some detail" },
                { $"{DefaultOptions.DimensionPrefix}.Instance", "Some instance" },
                { $"{DefaultOptions.DimensionPrefix}.Status", "400" },
                { $"{DefaultOptions.DimensionPrefix}.Title", "Some title" },
                { $"{DefaultOptions.DimensionPrefix}.Type", "Some type" }
            };
            d.Should().BeEquivalentTo(expected);
        }

        private static TestDefaultDimensionCollector Sut()
        {
            return new TestDefaultDimensionCollector(TestFixture.OptionsOf(TestFixture.DefaultOptions));
        }
    }
}