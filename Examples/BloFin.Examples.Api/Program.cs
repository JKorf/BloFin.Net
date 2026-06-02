using BloFin.Net;
using BloFin.Net.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the BloFin services
builder.Services.AddBloFin();

// OR to provide API credentials for accessing private endpoints, or setting other options:
/*
builder.Services.AddBloFin(options =>
{
    options.ApiCredentials = new BloFinCredentials("<APIKEY>", "<APISECRET>", "<APIPASS>");
    options.Rest.RequestTimeout = TimeSpan.FromSeconds(5);
});
*/

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map the endpoint and inject the rest client
app.MapGet("/{Symbol}", async ([FromServices] IBloFinRestClient client, string symbol) =>
{
    var result = await client.FuturesApi.ExchangeData.GetTickersAsync(symbol);
    if (!result.Success)
        return Results.Problem(result.Error?.Message, statusCode: 502);

    var ticker = result.Data.SingleOrDefault();
    return ticker == null
        ? Results.NotFound()
        : Results.Ok(ticker.LastPrice);
})
.WithOpenApi();


app.MapGet("/Balances", async ([FromServices] IBloFinRestClient client) =>
{
    var result = await client.FuturesApi.Account.GetBalancesAsync();
    return result.Success
        ? Results.Ok(result.Data)
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();

app.Run();
