using financial_control_infrastructure.Configuration;
using financial_control.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureInitializer()
    .ConfigureDI()
    .ConfigureDB()
    .ConfigureMessaging(builder.Configuration);

var app = builder.ConfigureHostedService().Build();

app.ConfigureMiddleware()
    .ConfigureSwagger()
    .ConfigureHttp();

if (!builder.Environment.IsEnvironment("Testing"))
    app.ConfigureMigration();

app.Run();

