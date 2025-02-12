using financial_control.Infraestructure.Context;
using financial_control.Infraestructure.Repository;
using financial_control.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<DbFinancialContext>();

builder.Services.AddScoped<PersonRepository>();
builder.Services.AddScoped<PersonService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();


