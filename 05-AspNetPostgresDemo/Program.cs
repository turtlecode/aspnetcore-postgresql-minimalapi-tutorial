using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string connectionString =
    "Host=localhost;Port=5432;Username=postgres;Password=turtlecode;Database=postgres";

app.MapGet("/", () =>
{
    return Results.Content("""
<html>
    <body>
        <h1>Add User</h1>
        <form method="post" action="/add">
            <input type="text" name="name" placeholder="Enter name" />
            <button type="submit">Save</button>
        </form>
    </body>
</html>
""", "text/html");
});

app.MapPost("/add", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    var name = form["name"].ToString();

    await using var conn = new NpgsqlConnection(connectionString);
    await conn.OpenAsync();

    await using var cmd =
        new NpgsqlCommand("INSERT INTO users (name) VALUES (@name)", conn);

    cmd.Parameters.AddWithValue("name", name);
    await cmd.ExecuteNonQueryAsync();

    return Results.Content("""
<html>
    <body>
        <h2>User Saved Successfully!</h2>
        <a href="/">Back</a>
    </body>
</html>
""", "text/html");
});

app.Run();