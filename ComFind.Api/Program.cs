var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        name = "ComFind API",
        status = "Running"
    });
});

app.Run();