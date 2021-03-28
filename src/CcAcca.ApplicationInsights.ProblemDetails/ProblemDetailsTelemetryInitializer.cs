using System.Collections.Generic;
using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MvcProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace CcAcca.ApplicationInsights.ProblemDetails
{
  internal class ProblemDetailsTelemetryInitializer : TelemetryInitializerBase
  {
    public const string HttpContextItemKey = "ProblemDetail";

    public ProblemDetailsTelemetryInitializer(IHttpContextAccessor httpContextAccessor,
      IOptionsMonitor<ProblemDetailsTelemetryOptions> options)
      : base(httpContextAccessor)
    {
      OptionsMonitor = options;
      DimensionCollector = new DefaultDimensionCollector(options);
    }

    public IDimensionCollector DimensionCollector { get; }

    private IOptionsMonitor<ProblemDetailsTelemetryOptions> OptionsMonitor { get; }

    protected override void OnInitializeTelemetry(
      HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
    {
      var httpContext = platformContext.Request.HttpContext;
      var options = OptionsMonitor.CurrentValue;

      if (!(httpContext.Items[HttpContextItemKey] is MvcProblemDetails problem))
      {
        return;
      }

      if (!options.ShouldSend(httpContext, problem))
      {
        return;
      }

      var choices = new DimensionChoices
      {
        IncludeErrorsValue = options.IncludeErrorsValue(httpContext, problem),
        IncludeExtensionsValue = options.IncludeExtensionsValue(httpContext, problem),
        IncludeRawJson = options.IncludeRawJson(httpContext, problem)
      };

      var candidateDimensions = new Dictionary<string, string>();
      DimensionCollector.CollectDimensions(candidateDimensions, problem, httpContext, choices);
      var dimensions = options.MapDimensions(httpContext, problem, candidateDimensions);

      if (dimensions == null)
      {
        return;
      }

      foreach (var (key, value) in candidateDimensions)
      {
        requestTelemetry.Properties[key] = value;
      }
    }
  }
}