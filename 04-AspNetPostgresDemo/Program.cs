using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string connectionString =
    "Host=localhost;Port=5432;Username=postgres;Password=turtlecode;Database=postgres";

app.MapGet("/", async () =>
{
    await using var conn = new NpgsqlConnection(connectionString);
    await conn.OpenAsync();

    await using var cmd = new NpgsqlCommand("SELECT NOW()", conn);
    var result = await cmd.ExecuteScalarAsync();

    return Results.Content($"""
<html>
    <body>
        <h1>PostgreSQL Connected</h1>
        <p>Server Time: {result}</p>
    </body>
</html>
""", "text/html");
});

app.Run();
