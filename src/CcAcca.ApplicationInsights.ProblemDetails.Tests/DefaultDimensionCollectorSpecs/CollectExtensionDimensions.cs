using System.Collections.Generic;
using CcAcca.ApplicationInsights.ProblemDetails;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Specs.DefaultDimensionCollectorSpecs
{
  public class CollectExtensionDimensions
  {
    private static ProblemDetailsTelemetryOptions DefaultOptions => TestFixture.DefaultOptions;

    [Fact]
    public void Empty()
    {
      // given
      var sut = Sut();
      var problem = new ProblemDetails();

      // when
      var d = new Dictionary<string, string>();
      sut.CollectExtensionDimensions(d, problem, null);

      // then
      d.Should().BeEmpty();
    }

    [Fact]
    public void One_item()
    {
      // given
      var sut = Sut();
      var problem = new ProblemDetails
      {
        Extensions = {{"Key1", "value1"}}
      };

      // when
      var d = new Dictionary<string, string>();
      sut.CollectExtensionDimensions(d, problem, null);

      // then
      var expected = new Dictionary<string, string>
      {
        {$"{DefaultOptions.DimensionPrefix}.Key1", "value1"}
      };
      d.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Pascal_case_key_should_be_converted_to_title_case()
    {
      // given
      var sut = Sut();
      var problem = new ProblemDetails
      {
        Extensions = {{"traceId", "value1"}}
      };

      // when
      var d = new Dictionary<string, string>();
      sut.CollectExtensionDimensions(d, problem, null);

      // then
      var expected = new Dictionary<string, string>
      {
        {$"{DefaultOptions.DimensionPrefix}.TraceId", "value1"}
      };
      d.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Duplicate_keys_different_by_case_should_collect_last_value()
    {
      // given
      var sut = Sut();
      var problem = new ProblemDetails
      {
        Extensions =
        {
          {"traceId", "value1"},
          {"TraceId", "value2"}
        }
      };

      // when
      var d = new Dictionary<string, string>();
      sut.CollectExtensionDimensions(d, problem, null);

      // then
      var expected = new Dictionary<string, string>
      {
        {$"{DefaultOptions.DimensionPrefix}.TraceId", "value2"}
      };
      d.Should().BeEquivalentTo(expected);
    }

    private static TestDefaultDimensionCollector Sut()
    {
      return new TestDefaultDimensionCollector(TestFixture.OptionsOf(TestFixture.DefaultOptions));
    }
  }
}