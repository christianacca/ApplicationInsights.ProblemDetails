using Microsoft.Extensions.Options;

namespace CcAcca.ApplicationInsights.ProblemDetails
{
  internal class ProblemDetailsTelemetryOptionsSetup : IPostConfigureOptions<ProblemDetailsTelemetryOptions>
  {
    public void PostConfigure(string name, ProblemDetailsTelemetryOptions options)
    {
      options.IncludeErrorsValue ??= (context, problem) => true;
      options.IncludeExtensionsValue ??= (context, problem) => true;
      options.IncludeRawJson ??= (context, problem) => true;
      options.MapDimensions ??= (context, problem, dimensions) => dimensions;
      options.SerializeValue ??= DefaultDimensionCollector.SerializeValue;
      options.ShouldSend ??= (context, problem) => true;
    }
  }
}