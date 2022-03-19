using System.Runtime.ExceptionServices;
using fruit_service;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Fruit Service", Version = "v1" });
});

const string SERVICE_NAME = "fruit-service";
const string SERVICE_VERSION = "1.0.0";
var resourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName: SERVICE_NAME, serviceVersion: SERVICE_VERSION).AddTelemetrySdk();
builder.Services.AddHttpClient();

builder.Services.AddOpenTelemetryTracing(b => {
    b.SetResourceBuilder(resourceBuilder)
    .AddAspNetCoreInstrumentation()
    .AddHttpClientInstrumentation()
    .AddConsoleExporter()
    .AddOtlpExporter(options =>
    {
        options.Endpoint = new Uri($"{Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT")}");
        options.Headers = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_HEADERS");
    });
});

builder.Services.AddOpenTelemetryMetrics(b => {
    b.SetResourceBuilder(resourceBuilder)
    .AddAspNetCoreInstrumentation()
    .AddConsoleExporter()
    .AddHttpClientInstrumentation()
    .AddOtlpExporter(options =>
    {
        options.Endpoint = new Uri($"{Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT")}");
        options.Headers = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_HEADERS");
    });
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fruit Service v1"));

app.MapGet("/fruits", () => {
    return Results.Ok(Enumerable.Range(1, 5).Select(index => new Fruit
    {
        Name = Fruit.SampleNames[Random.Shared.Next(Fruit.SampleNames.Length)]
    })
    .ToArray());
});

// Helps debug some opentel errors
AppDomain.CurrentDomain.FirstChanceException +=
            (object source, FirstChanceExceptionEventArgs e) =>
            {
                Console.WriteLine("FirstChanceException event raised in {0}: {1}",
                    AppDomain.CurrentDomain.FriendlyName, e.Exception.Message);
            };

app.Run();
