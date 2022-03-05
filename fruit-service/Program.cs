using fruit_service;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Fruit Service", Version = "v1" });
});

const string SERVICE_NAME = "fruit-service";
const string SERVICE_VERSION = "1.0.0";
const string OTLP_TRACING_ENDPOINT = "https://otlp.nr-data.net:4318/v1/traces";
const string OTLP_METRIC_ENDPOINT = "https://otlp.nr-data.net:4318/v1/metrics";
var newRelicApiKey = builder.Configuration.GetSection("NEW_RELIC").GetValue<string>("API_KEY");
var resourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName: SERVICE_NAME, serviceVersion: SERVICE_VERSION).AddTelemetrySdk();

builder.Services.AddOpenTelemetryTracing(b => {
    b.SetResourceBuilder(resourceBuilder)
    .AddOtlpExporter(options => options.Endpoint = new Uri(OTLP_TRACING_ENDPOINT))
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation();
});

builder.Services.AddOpenTelemetryMetrics(b => {
    b.SetResourceBuilder(resourceBuilder)
    .AddOtlpExporter(options => options.Endpoint = new Uri(OTLP_METRIC_ENDPOINT))
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation();
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
app.Run();
