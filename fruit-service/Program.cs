using fruit_service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Fruit Service", Version = "v1" });
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fruit Service v1"));
app.UseHttpsRedirection();

app.MapGet("/fruits", () => {
    return Results.Ok(Enumerable.Range(1, 5).Select(index => new Fruit
    {
        Name = Fruit.SampleNames[Random.Shared.Next(Fruit.SampleNames.Length)]
    })
    .ToArray());
});
app.Run();
