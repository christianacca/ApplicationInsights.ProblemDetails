using CcAcca.ApplicationInsights.ProblemDetails;
using Microsoft.Extensions.Options;
using Moq;

namespace Specs.DefaultDimensionCollectorSpecs
{
  public static class TestFixture
  {
    private static ProblemDetailsTelemetryOptions _defaultOptions;

    public static ProblemDetailsTelemetryOptions DefaultOptions
    {
      get
      {
        if (_defaultOptions == null)
        {
          var setup = new ProblemDetailsTelemetryOptionsSetup();
          var options = new ProblemDetailsTelemetryOptions();
          setup.PostConfigure("", options);
          _defaultOptions = options;
        }

        return _defaultOptions;
      }
    }

    public static IOptionsMonitor<ProblemDetailsTelemetryOptions> OptionsOf(ProblemDetailsTelemetryOptions options)
    {
      var mock = new Mock<IOptionsMonitor<ProblemDetailsTelemetryOptions>>();
      mock.Setup(o => o.CurrentValue).Returns(options);
      return mock.Object;
    }
  }
}