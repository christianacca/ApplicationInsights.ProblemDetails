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
    ///   Gets or sets the function that can be used to override the key-value pairs
    ///   (aka custom dimensions) that will be used to enrich the request telemetry
    /// </summary>
    public Func<HttpContext, MvcProblemDetails, Dimensions, Dimensions> MapDimensions { get; set; }


    /// <summary>
    ///   Gets or sets the function for serializing an object to be sent as a custom dimension
    /// </summary>
    /// <remarks>
    ///   The default implementation will serialize primitive and primitive-like values (eg DateTime)
    ///   using their <see cref="object.ToString" /> method and for all other types to try and serialize
    ///   the object as a JSON string
    /// </remarks>
    public Func<HttpContext, MvcProblemDetails, string, object, string> SerializeValue { get; set; }
  }
}