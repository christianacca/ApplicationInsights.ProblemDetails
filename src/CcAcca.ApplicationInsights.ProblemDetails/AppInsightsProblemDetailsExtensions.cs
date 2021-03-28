using System;
using Hellang.Middleware.ProblemDetails;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvcProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace CcAcca.ApplicationInsights.ProblemDetails
{
  public static class AppInsightsProblemDetailsExtensions
  {
    /// <summary>
    ///   Enrich request telemetry with custom dimensions extracted from the <see cref="MvcProblemDetails" />
    ///   that will be sent as a response.
    /// </summary>
    public static IServiceCollection AddProblemDetailTelemetryInitializer(this IServiceCollection services)
    {
      return services.AddProblemDetailTelemetryInitializer(null);
    }

    /// <summary>
    ///   Enrich request telemetry with custom dimensions extracted from the <see cref="MvcProblemDetails" />
    ///   that will be sent as a response.
    ///   Uses the specified <paramref name="configure" /> callback for configuration.
    /// </summary>
    public static IServiceCollection AddProblemDetailTelemetryInitializer(this IServiceCollection services,
      Action<ProblemDetailsTelemetryOptions> configure)
    {
      // Add ASP.NET Core Options libraries - just in case our consumer hasn't (safe to call multiple times)
      services.AddOptions();

      if (configure != null)
      {
        services.Configure(configure);
      }

      services.AddSingleton<ITelemetryInitializer, ProblemDetailsTelemetryInitializer>();
      services.TryAddSingleton<IDimensionCollector, DefaultDimensionCollector>();
      services.ConfigureOptions<ProblemDetailsTelemetryOptionsSetup>();

      services.PostConfigure<ProblemDetailsOptions>(options => {
        options.OnBeforeWriteDetails += (context, problem) => {
          context.Items[ProblemDetailsTelemetryInitializer.HttpContextItemKey] = problem;
        };
      });

      return services;
    }
  }
}