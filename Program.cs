var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();              //MemoryCache that keeps the Shorten urls mapping

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();
app.UseMiddleware<RedirectMiddleware>();        //Add the Middlewre that does the redirect

app.Run();
