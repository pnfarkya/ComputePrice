using ComputePrice.Data;
using ComputePrice.Repository;
using Microsoft.EntityFrameworkCore;

var corsPolicy = "_allowedOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
                      policy =>
                      {
                          policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ComputPriceDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("ComputePriceDb")));
builder.Services.AddTransient<IComputePriceRepo, ComputePriceRepo>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicy);
app.UseAuthorization();

app.MapControllers();

app.Run();
