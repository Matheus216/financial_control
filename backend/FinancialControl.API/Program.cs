using FinancialControl.API.Configuration;
using FinancialControl.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var keycloakSection = builder.Configuration.GetSection("keycloak"); 

builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority =  keycloakSection["Authority"];
        options.Audience = keycloakSection["Audience"];
        options.RequireHttpsMetadata = bool.Parse(keycloakSection["RequireHttpsMetadata"]!);
    });

builder.Services
    .AddAuthorization()
    .ConfigureKeys(builder.Configuration)
    .ConfigureHttp(builder.Configuration)
    .ConfigureEndpoint()
    .ConfigureDI();

WebApplication app = builder.Build().MapVersion1Api();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.Run();
