using TicTacToe.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services
       .AddConfiguredControllers()
       .AddConfiguredSwagger()
       .AddConfiguredRedisGameRepository(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseSwagger().UseSwaggerUI();
app.MapControllers();
app.MapGet("/", (IConfiguration _) => Results.LocalRedirect("/swagger"));

app.Run();