using System.Collections.Generic;
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

        public new void CollectionStandardDimensions(IDictionary<string, string> dimensions, ProblemDetails problem,
            HttpContext httpContext)
        {
            base.CollectionStandardDimensions(dimensions, problem, httpContext);
        }

        public new void CollectExtensionDimensions(IDictionary<string, string> dimensions, ProblemDetails problem,
            HttpContext httpContext)
        {
            base.CollectExtensionDimensions(dimensions, problem, httpContext);
        }

        public new void CollectValidationErrorDimensions(
            IDictionary<string, string> dimensions, ValidationProblemDetails problem, HttpContext httpContext)
        {
            base.CollectValidationErrorDimensions(dimensions, problem, httpContext);
        }
    }
}