using Npgsql;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string connectionString =
    "Host=localhost;Port=5432;Username=postgres;Password=turtlecode;Database=postgres";

app.MapGet("/", async () =>
{
    var html = new StringBuilder();
    html.Append("""
<html>
    <body>
        <h1>User List</h1>
        <form method="post" action="/add">
            <input type="text" name="name" placeholder="Enter name" />
            <button type="submit">Save</button>
        </form>
        <hr/>
""");

    await using var conn = new NpgsqlConnection(connectionString);
    await conn.OpenAsync();

    await using var cmd =
        new NpgsqlCommand("SELECT id, name FROM users ORDER BY id DESC", conn);

    await using var reader = await cmd.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        html.Append($"<p>{reader["id"]} - {reader["name"]}</p>");
    }

    html.Append("""
    </body>
</html>
""");

    return Results.Content(html.ToString(), "text/html");
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

    return Results.Redirect("/");
});

app.Run();
