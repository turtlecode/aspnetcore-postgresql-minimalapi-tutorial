var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", (string? name) =>
{
    var message = string.IsNullOrEmpty(name)
        ? "Please enter your name."
        : $"Hello {name}!";

    return Results.Content($"""
<html>
    <body>
        <h1>Step 3 - Basic Form</h1>
        <form method="get">
            <input type="text" name="name" placeholder="Enter your name" />
            <button type="submit">Send</button>
        </form>
        <p>{message}</p>
    </body>
</html>
""", "text/html");
});

app.Run();