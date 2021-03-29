# Introduction 

MS Application Insights integration for the Hellang.Middleware.ProblemDetails package.

* Enriches request telementry with custom dimensions extracted from a ProblemDetail response.

## Develop

To build and run tests you can use:
* the dotnet cli tool
* any IDE/editor that understands MSBuild eg Visual Studio or Visual Studio Code

**Recommended workflow**

* Develop on a feature branch created from master:
    * create a branch from *master*.
    * perform all the code changes into the newly created branch.
    * merge *master* into your branch, then run tests locally (eg `dotnet test src/CcAcca.ApplicationInsights.ProblemDetails.Tests`)
    * on the new branch, bump the version number in 
      [CcAcca.ApplicationInsights.ProblemDetails.csproj](src/CcAcca.ApplicationInsights.ProblemDetails/CcAcca.ApplicationInsights.ProblemDetails.csproj); follow 
      [semver](https://semver.org/)
    * raise the PR (pull request) for code review & merge request to master branch.
    * PR will auto trigger a limited CI build (compile and test only)
    * approval of the PR will merge your branch code changes into the *master*

## CI server

Azure Devops is used to run the dotnet cli tool to perform the build and test. See the [yarn build definition](.azure-pipelines.yml) for details.

Notes:
* The CI build is configured to run on every commit to any branch
* PR completion to master will also publish the nuget package for CcAcca.ApplicationInsights.ProblemDetails to [Nuget gallery](https://www.nuget.org/)