using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;
using Dimensions = System.Collections.Generic.IDictionary<string, string>;

namespace CcAcca.ApplicationInsights.ProblemDetails
{
  public class ProblemDetailsTelemetryOptions
  {
    /// <summary>
    ///   The prefix for all custom dimensions used to enrich telemetry
    /// </summary>
    public string DimensionPrefix { get; set; } = "Problem";

    /// <summary>
    ///   Gets or sets the function for determining whether the ProblemDetails object
    ///   for the current request should be used to enrich the request telemetry.
    ///   The default is to always send
    /// </summary>
    public Func<HttpContext, MvcProblemDetails, bool> ShouldSend { get; set; }

    /// <summary>
    ///   Gets or sets the function for determining whether each key-value pair from
    ///   <see cref="ValidationProblemDetails.Errors" /> be included in the custom dimensions
    ///   sent to app insights.
    /// </summary>
    /// <remarks>
    ///   The default is to always include this dimension
    /// </remarks>
    public Func<HttpContext, MvcProblemDetails, bool> IncludeErrorsValue { get; set; }

    /// <summary>
    ///   Gets or sets the function for determining whether each key-value pair from
    ///   <see cref="MvcProblemDetails.Extensions" /> be included in the custom dimensions
    ///   sent to app insights.
    /// </summary>
    /// <remarks>
    ///   The default is to send all values from <see cref="MvcProblemDetails.Extensions" />
    ///   dictionary that <see cref="SerializeValue" /> returns as string
    /// </remarks>
    public Func<HttpContext, MvcProblemDetails, bool> IncludeExtensionsValue { get; set; }

    /// <summary>
    ///   Gets or sets the function for determining whether the JSON serialized
    ///   <see cref="MvcProblemDetails" /> object be included in the custom dimensions
    ///   sent to app insights.
    /// </summary>
    /// <remarks>
    ///   The default is to always include this dimension
    /// </remarks>
    public Func<HttpContext, MvcProblemDetails, bool> IncludeRawJson { get; set; }
    
    /// <summary>
    ///   Gets or sets the function for determining whether the <see cref="MvcProblemDetails" />
    ///   should be considered a success/failure within application insights
    /// </summary>
    public Func<HttpContext, MvcProblemDetails, bool> IsFailure { get; set; }

    /// <summary>
    ///   Gets or sets the function that can be used to override the key-value pairs
    ///   (aka custom dimensions) that will be used to enrich the request telemetry
    /// </summary>
    public Func<HttpContext, MvcProblemDetails, Dimensions, Dimensions> MapDimensions { get; set; }


    /// <summary>
    ///   Gets or sets the function for serializing an object to be sent as a custom dimension
    /// </summary>
    /// <remarks>
    ///   The default implementation will serialize primitive and primitive-like values (eg DateTime)
    ///   method and for all other types to try and serialize
    ///   the object as a JSON string
    /// </remarks>
    public Func<HttpContext, MvcProblemDetails, string, object, string> SerializeValue { get; set; }


    /// <summary>
    /// Function that will consider all <see cref="MvcProblemDetails" /> with a status code of 500 or above
    /// as a failure
    /// </summary>
    /// <remarks>
    /// Assign this to <see cref="IsFailure"/> to override the default behaviour in application insights
    /// that will treat any request that returns a status code greater or equal to 400 as NOT successful
    /// </remarks>
    public static Func<HttpContext, MvcProblemDetails, bool> ServerErrorIsFailure =
      (context, problem) => problem.Status >= StatusCodes.Status500InternalServerError;
  }
}