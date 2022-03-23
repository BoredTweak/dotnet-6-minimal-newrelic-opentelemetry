# dotnet-6-minimal-newrelic-opentelemetry
Demonstration of .NET 6 minimal APIs with New Relic's OpenTelemetry harvesting


## New Relic

- [Blog Post](https://newrelic.com/blog/best-practices/new-relic-opentelemetry-net)
- [Documentation](https://docs.newrelic.com/docs/more-integrations/open-source-telemetry-integrations/opentelemetry/opentelemetry-quick-start/)

## Prerequisites

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Docker](https://docs.docker.com/get-docker/)

## Configuration

This program requires two environment variables to enable the OpenTelemetry libraries.
  - OTEL_EXPORTER_OTLP_ENDPOINT
    - Set this to `https://otlp.nr-data.net:4317`
  - OTEL_EXPORTER_OTLP_HEADERS
    - Set this to `api-key=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX` replacing `XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX` with a valid New Relic Ingest License key found [here](https://one.newrelic.com/admin-portal/api-keys/home).

You can also set these environment variables in the .env file locally and run via Docker Compose.

## Running locally

- If you set the environment variables on your local environment, you can run this program via `dotnet run` from a terminal instances in the [fruit-service](fruit-service/) directory.
- If you set the environment variables in the [.env](.env) file then you can run this program via `docker compose --env-file .env up`