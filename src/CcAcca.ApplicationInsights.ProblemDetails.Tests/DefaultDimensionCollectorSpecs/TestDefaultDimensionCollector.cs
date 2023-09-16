using CcAcca.ApplicationInsights.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Specs.DefaultDimensionCollectorSpecs
{
    internal class TestDefaultDimensionCollector : DefaultDimensionCollector
    {
        public TestDefaultDimensionCollector(IOptionsMonitor<ProblemDetailsTelemetryOptions> optionsMonitor) : base(
            optionsMonitor)
        {
        }

        public new void CollectionStandardDimensions(
            IDictionary<string, string> dimensions, ProblemDetails problem, HttpContext? httpContext = null)
        {
            base.CollectionStandardDimensions(dimensions, problem, httpContext ?? new DefaultHttpContext());
        }

        public new void CollectExtensionDimensions(
            IDictionary<string, string> dimensions, ProblemDetails problem, HttpContext? httpContext = null)
        {
            base.CollectExtensionDimensions(dimensions, problem, httpContext ?? new DefaultHttpContext());
        }

        public new void CollectValidationErrorDimensions(
            IDictionary<string, string> dimensions, ValidationProblemDetails problem, HttpContext? httpContext = null)
        {
            base.CollectValidationErrorDimensions(dimensions, problem, httpContext ?? new DefaultHttpContext());
        }
    }
}