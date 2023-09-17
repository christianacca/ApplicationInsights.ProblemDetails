using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MvcProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace CcAcca.ApplicationInsights.ProblemDetails
{
    public interface IDimensionCollector
    {
        /// <summary>
        ///     Collect the properties from <paramref name="problem" /> and add these to <paramref name="dimensions" />
        /// </summary>
        /// <param name="dimensions">The dictionary to add dimensions to</param>
        /// <param name="problem">The problem instance from whom dimensions are being collected</param>
        /// <param name="httpContext">The http context of the current request</param>
        /// <param name="choices">Determines the properties of the <paramref name="problem" /> that should be collected</param>
        void CollectDimensions(
            IDictionary<string, string> dimensions,
            MvcProblemDetails problem,
            HttpContext httpContext,
            DimensionChoices choices);
    }

    /// <summary>
    ///     Default implementation that will collect dimensions from the properties of
    ///     a <see cref="MvcProblemDetails" /> instance
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Consider configuring an instance of <see cref="ProblemDetailsTelemetryOptions" /> first
    ///         rather than replace this implement of the <see cref="IDimensionCollector" />.
    ///     </para>
    ///     <para>
    ///         If you need to customize the implement of <see cref="IDimensionCollector" />,
    ///         the easiest option is to inherit from this class and override one or more
    ///         of the protected methods.
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    /// public class CustomDefaultDimensionCollector : DefaultDimensionCollector
    /// {
    ///   protected override void CollectExtensionDimensions(
    ///     IDictionary&lt;string, string> dimensions, MvcProblemDetails problem, HttpContext httpContext)
    ///   {
    ///     // your implementation
    ///   }
    /// }
    /// 
    /// // Startup.ConfigureServices method
    /// services.TryAddSingleton&lt;IDimensionCollector, CustomDefaultDimensionCollector>();
    /// </code>
    /// </example>
    public class DefaultDimensionCollector : IDimensionCollector
    {
        public const string RawDimensionKey = "Raw";

        public DefaultDimensionCollector(IOptionsMonitor<ProblemDetailsTelemetryOptions> optionsMonitor)
        {
            OptionsMonitor = optionsMonitor;
        }

        private static JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private IOptionsMonitor<ProblemDetailsTelemetryOptions> OptionsMonitor { get; }
        public ProblemDetailsTelemetryOptions Options => OptionsMonitor.CurrentValue;

        public virtual void CollectDimensions(
            IDictionary<string, string> dimensions,
            MvcProblemDetails problem,
            HttpContext httpContext,
            DimensionChoices choices)
        {
            CollectionStandardDimensions(dimensions, problem, httpContext);

            if (problem is ValidationProblemDetails validationProblem && choices.IncludeErrorsValue)
            {
                CollectValidationErrorDimensions(dimensions, validationProblem, httpContext);
            }

            if (choices.IncludeExtensionsValue)
            {
                CollectExtensionDimensions(dimensions, problem, httpContext);
            }

            if (choices.IncludeRawJson)
            {
                CollectionRawProblemDimension(dimensions, problem, httpContext);
            }
        }

        /// <summary>
        ///     Add the values in the <see cref="MvcProblemDetails.Extensions" /> property of the
        ///     <paramref name="problem" /> to the <paramref name="dimensions" />
        /// </summary>
        protected virtual void CollectExtensionDimensions(
            IDictionary<string, string> dimensions, MvcProblemDetails problem, HttpContext httpContext)
        {
            foreach (var (key, value) in problem.Extensions)
            {
                CollectOne(dimensions, problem, UppercaseFirst(key), value, httpContext);
            }
        }

        /// <summary>
        ///     Serialize the <paramref name="value" /> and add any non-null result as a custom dimension using a
        ///     key constructed from the <see cref="ProblemDetailsTelemetryOptions.DimensionPrefix" />
        ///     and <paramref name="key" />
        /// </summary>
        /// <param name="dimensions">The dictionary to add dimensions to</param>
        /// <param name="problem">The problem instance from whom dimensions are being collected</param>
        /// <param name="key">The name of the property or error key</param>
        /// <param name="value">The value to serialized and added as a dimension</param>
        /// <param name="httpContext">The http context of the current request</param>
        protected virtual void CollectOne(IDictionary<string, string> dimensions, MvcProblemDetails problem,
            string key,
            object? value, HttpContext httpContext)
        {
            var serializedValue = Options.SerializeValue(httpContext, problem, key, value);
            if (serializedValue != null)
            {
                dimensions[$"{Options.DimensionPrefix}.{key}"] = serializedValue;
            }
        }

        /// <summary>
        ///     Serialize the <paramref name="problem" /> and add this to <paramref name="dimensions" />
        /// </summary>
        protected virtual void CollectionRawProblemDimension(
            IDictionary<string, string> dimensions, MvcProblemDetails problem, HttpContext httpContext)
        {
            CollectOne(dimensions, problem, RawDimensionKey, problem, httpContext);
        }

        /// <summary>
        ///     Add the properties from the <paramref name="problem" /> that are defined in the problem-details
        ///     specification to the <paramref name="dimensions" />
        /// </summary>
        protected virtual void CollectionStandardDimensions(
            IDictionary<string, string> dimensions, MvcProblemDetails problem, HttpContext httpContext)
        {
            CollectOne(dimensions, problem, nameof(MvcProblemDetails.Type), problem.Type, httpContext);
            CollectOne(dimensions, problem, nameof(MvcProblemDetails.Status), problem.Status ?? 0, httpContext);
            CollectOne(dimensions, problem, nameof(MvcProblemDetails.Detail), problem.Detail, httpContext);
            CollectOne(dimensions, problem, nameof(MvcProblemDetails.Title), problem.Title, httpContext);
            CollectOne(dimensions, problem, nameof(MvcProblemDetails.Instance), problem.Instance, httpContext);
        }

        /// <summary>
        ///     Add the values in the <see cref="ValidationProblemDetails.Errors" /> property of the
        ///     <paramref name="problem" /> to the <paramref name="dimensions" />
        /// </summary>
        protected virtual void CollectValidationErrorDimensions(
            IDictionary<string, string> dimensions, ValidationProblemDetails problem, HttpContext httpContext)
        {
            foreach (var (key, value) in problem.Errors)
            {
                const string errorsKey = nameof(ValidationProblemDetails.Errors);
                var errorKey = string.IsNullOrEmpty(key) ? errorsKey : $"{errorsKey}.{key}";
                CollectOne(dimensions, problem, errorKey, string.Join("; ", value), httpContext);
            }
        }

        /// <summary>
        ///     The default implementation used to serialize value to be sent as custom dimensions
        /// </summary>
        /// <returns></returns>
        public static string? SerializeValue(HttpContext httpContext, MvcProblemDetails problem, string key,
            object? value)
        {
            return value switch
            {
                null => null,
                string s => s,
                int _ => value.ToString(),
                DateTime dtm => dtm.ToString("O"),
                DateTimeOffset dtm2 => dtm2.ToString("O"),
                DateOnly d => d.ToString("O"),
                TimeOnly t => t.ToString("O"),
                _ => TrySerializeAsJson(value)
            };
        }

        public static string? TrySerializeAsJson(object? value)
        {
            try
            {
                return JsonSerializer.Serialize(value, SerializerOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        public static string UppercaseFirst(string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return string.Empty;
            }

            if (s.Length == 1)
            {
                return char.ToUpper(s[0]).ToString();
            }

            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}