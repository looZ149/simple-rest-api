using SampleTracker.model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


//We just define a hardcoded list for now
var samples = new List<Sample>
{
    new Sample { Id = 1, FileName = "dropper.exe", Sha256 = "abcdef173", Note = "Themida Packed"},
    new Sample { Id = 2, FileName = "loader.dll", Sha256 = "defgawd37213", Note = "VMProtect"}
};

app.MapGet("/samples", () => samples);

app.MapGet("/samples/{id:int}", (int id) =>
{
    var sample = samples.FirstOrDefault(s => s.Id == id);

    return sample is not null
        ? Results.Ok(sample)
        : Results.NotFound();
});

// ASP.NET automatically deserializes the JSON request body into a full sample object 
app.MapPost("/samples", (Sample sample) =>
{
    sample.Id = samples.Count == 0 ? 1 : samples.Max(s => s.Id!.Value) + 1;
    samples.Add(sample);
    return Results.Created($"/samples/{sample.Id}", sample);
});

app.MapDelete("samples/{id:int}", (int id) =>
{
    var sample = samples.FirstOrDefault(s => s.Id == id);
    if (sample is null)
        return Results.NotFound();

    samples.Remove(sample);
    return Results.NoContent();
});

app.Run();


