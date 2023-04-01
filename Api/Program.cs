using Api;
using DotNetEnv;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

switch (environment)
{
    case "Development":
        Env.Load(".env.development");
        break;

    case "Staging":
        Env.Load(".env.staging");
        break;

    case "Production":
        Env.Load(".env.production");
        break;

    default:
        Env.Load(".env");
        break;
}

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();
