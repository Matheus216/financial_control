using FinancialControl.API.Configuration;
using FinancialControl.API.Data;
using FinancialControl.API.Endpoints;
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

builder.Services
    .ConfigureKeys(builder.Configuration)
    .ConfigureHttp(builder.Configuration)
    .ConfigureDI();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

var apiGroup = app.MapGroup("/api")
    .MapAssetEndpoints()
    .MapOrderEndpoints()
    .MapPersonEndpoints()
    .MapWalletEndpoints()
    .MapRevenueEndpoints()
    .MapMovementEndpoints(); 
app.Run();
