# Changelog

All notable changes to this project will be documented in this file.

## [2.0.0-beta.1] - 2023-10-17

### Added

- Support for nullable reference types
- `DefaultDimensionCollector`: `SerializeValue` supports `DateOnly` and `TimeOnly`

### Breaking Changes

- Upgraded Microsoft.ApplicationInsights.AspNetCore dependency to 2.21.0
  - This to mitigate vulnerability in Newtonsoft.Json see: https://github.com/advisories/GHSA-5crp-9r3c-p9vr
- Dropped support for .NET Core 3.1; minimum supported .NET version is now .NET 6.0
- Signature of dimension serialization changed to support nullable reference types

## [1.1.1] - 2021-12-02

### Fixed

- change Microsoft.ApplicationInsights.AspNetCore minimum version to 2.12.0

## [1.1.0] - 2021-06-02

### Added

- `ProblemDetailsTelemetryOptions`: new `IsFailure` option used to determine whether a ProblemDetail should be considered a success/failure
- `ProblemDetailsTelemetryOptions`: new `MergeInto` convenience method

## [1.0.2] - 2021-06-01

### Fixed

- Dimension collection not respecting `ProblemDetailsTelemetryOptions.MapDimensions`

## [1.0.1] - 2021-04-20

### Fixed

- Raw JSON ProblemDetails value not serialized correctly

## [1.0.0] - 2021-03-29

### Added

- Initial release
