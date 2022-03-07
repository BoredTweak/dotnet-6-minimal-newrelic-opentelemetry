using fruit_service;
using OpenTelemetry.Exporter;
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
var otlpEndpoint = builder.Configuration.GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT");
var newRelicApiKey = builder.Configuration.GetSection("NEW_RELIC").GetValue<string>("API_KEY");
var resourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName: SERVICE_NAME, serviceVersion: SERVICE_VERSION).AddTelemetrySdk();

builder.Services.AddOpenTelemetryTracing(b => {
    b.SetResourceBuilder(resourceBuilder)
    .AddAspNetCoreInstrumentation()
    .AddConsoleExporter()
    .AddOtlpExporter(options => {
        options.Protocol = OtlpExportProtocol.HttpProtobuf;
        options.Endpoint = new Uri(otlpEndpoint);
        options.HttpClientFactory = () =>
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("api-key", newRelicApiKey);
            return client;
        };
        });
});

builder.Services.AddOpenTelemetryMetrics(b => {
    b.SetResourceBuilder(resourceBuilder)
    .AddAspNetCoreInstrumentation()
    .AddConsoleExporter()
    .AddOtlpExporter(options => {
        options.Protocol = OtlpExportProtocol.HttpProtobuf;
        options.Endpoint = new Uri(otlpEndpoint);
        options.HttpClientFactory = () =>
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("api-key", newRelicApiKey);
            return client;
        };
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
app.Run();
