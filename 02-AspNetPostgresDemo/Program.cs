var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Content("""
<html>
    <body>
        <h1>Hello World!</h1>
        <p>ASP.NET Core is running.</p>
    </body>
</html>
""", "text/html"));

app.Run();