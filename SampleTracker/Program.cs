using SampleTracker.Model;
using SampleTracker.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// DI to set up the Connection properly
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Auto migrates the DB on every startup
using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/samples", async (AppDbContext db) => 
    await db.Samples.ToListAsync());

app.MapGet("/samples/{id:int}", async (AppDbContext db, int id) => {
    var sample = await db.Samples.FirstOrDefaultAsync(s => s.Id == id);

    return sample is not null
        ? Results.Ok(sample)
        : Results.NotFound();
});

app.MapPost("/samples", async (AppDbContext db, Sample sample) => {
    if (sample.FileName is null)
        return Results.BadRequest();
    if (sample.Note is null)
        sample.Note = "No note added";

    if (sample.Sha256 is not null && await db.Samples.AnyAsync(s => s.Sha256 == sample.Sha256))
        return Results.Conflict();

    db.Samples.Add(sample);
    await db.SaveChangesAsync();
    return Results.Created($"/samples/{sample.Id}", sample);
});

app.MapPatch("/samples/{id:int}", async (AppDbContext db, int id, Sample patch) => {
    var sample = await db.Samples.FirstOrDefaultAsync(s => s.Id == id);
    if (sample is null)
        return Results.NotFound();
    if (patch.FileName is not null)
        sample.FileName = patch.FileName;
    if (patch.Sha256 is not null)
        sample.Sha256 = patch.Sha256;
    if (patch.Note is not null)
        sample.Note = patch.Note;

    await db.SaveChangesAsync();
    return Results.Ok(sample);

});

app.MapDelete("/samples/{id:int}", async (AppDbContext db, int id) => {
    var sample = await db.Samples.FirstOrDefaultAsync(s => s.Id == id);
    if (sample is null)
        return Results.NotFound();

    db.Samples.Remove(sample);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();