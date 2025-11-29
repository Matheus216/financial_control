using FinancialControl.API.Data;
using FinancialControl.API.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAssetEndpoints();
app.MapOrderEndpoints();
app.MapPeopleEndpoints();
app.MapWalletEndpoints();
app.MapRevenueEndpoints();
app.MapMovementEndpoints();

app.Run();
