﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Travelephant;
using Travelephant.Data;
using Travelephant.Helper;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TravelephantContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TravelephantContext") ?? throw new InvalidOperationException("Connection string 'TravelephantContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//to get the secret key for jwt authentication
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("ApplicationSettings"));

builder.Services.AddScoped<JwtService>();

//var provider = builder.Services.BuildServiceProvider();
//var configuration = provider.GetRequiredService<IConfiguration>();



builder.Services.AddCors(options =>
{
    var frontendURL = builder.Configuration.GetValue<string>("frontend_url");
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors(options => options
.WithOrigins(new[] { "http:/localhost:3000" })
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
);

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<TravelephantContext>();
db.Database.Migrate();

app.Run();
