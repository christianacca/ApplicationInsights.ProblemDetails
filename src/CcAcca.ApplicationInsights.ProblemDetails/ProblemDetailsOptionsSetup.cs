using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.Options;

namespace CcAcca.ApplicationInsights.ProblemDetails
{
    internal class ProblemDetailsOptionsSetup : IPostConfigureOptions<ProblemDetailsOptions>
    {
        public ProblemDetailsOptionsSetup(IOptionsMonitor<ProblemDetailsTelemetryOptions> libraryOptions)
        {
            LibraryOptions = libraryOptions;
        }

        private IOptionsMonitor<ProblemDetailsTelemetryOptions> LibraryOptions { get; }

        public void PostConfigure(string name, ProblemDetailsOptions options)
        {
            options.OnBeforeWriteDetails += (context, problem) => {
                context.Items[ProblemDetailsTelemetryInitializer.HttpContextProblemItemKey] = problem;
                if (LibraryOptions.CurrentValue.IsFailure != null)
                {
                    context.Items[ProblemDetailsTelemetryInitializer.HttpContextIsFailureItemKey] =
                        LibraryOptions.CurrentValue.IsFailure(context, problem);
                }
            };
        }
    }
}