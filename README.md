# dotnet-6-minimal-newrelic-opentelemetry
Demonstration of .NET 6 minimal APIs with New Relic's OpenTelemetry harvesting


## New Relic

- [Blog Post](https://newrelic.com/blog/best-practices/new-relic-opentelemetry-net)
- [Documentation](https://docs.newrelic.com/docs/more-integrations/open-source-telemetry-integrations/opentelemetry/opentelemetry-quick-start/)

## Prerequisites

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Docker](https://docs.docker.com/get-docker/)

## Configuration

- From the [fruit-service](./fruit-service/) directory, run `dotnet user-secrets set NEW_RELIC__API_KEY "NRAK-XXXXXXXXXX"` replacing `NRAK-XXXXXXXXXX` with your New Relic API key.

