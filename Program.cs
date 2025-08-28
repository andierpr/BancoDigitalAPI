using BancoDigitalAPI.Data;
using BancoDigitalAPI.GraphQL.Mutations;
using BancoDigitalAPI.GraphQL.Queries;
using BancoDigitalAPI.Models;
using BancoDigitalAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext com MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))));

builder.Services.AddScoped<ContaService>();

// GraphQL
builder.Services.AddGraphQLServer()
    .AddQueryType<ContaQuery>()
    .AddMutationType<ContaMutation>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BancoDigitalAPI V1");
         c.RoutePrefix = string.Empty; // <--- deixa o Swagger na raiz
    });
}

app.UseHttpsRedirection();

// GraphQL endpoint
app.MapGraphQL();

// Endpoints REST
app.MapPost("/api/criar-conta", async (int conta, decimal saldoInicial, ContaService service) =>
{
    try
    {
        var novaConta = await service.CriarContaAsync(conta, saldoInicial);
        return Results.Ok(novaConta);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});



app.MapPost("/api/depositar", async (int conta, decimal valor, ContaService service) =>
{
    try
    {
        var result = await service.DepositarAsync(conta, valor);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPost("/api/sacar", async (int conta, decimal valor, ContaService service) =>
{
    try
    {
        var result = await service.SacarAsync(conta, valor);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/api/saldo/{conta}", async (int conta, ContaService service) =>
{
    try
    {
        var saldo = await service.ObterSaldoAsync(conta);
        return Results.Ok(new { conta, saldo });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();
