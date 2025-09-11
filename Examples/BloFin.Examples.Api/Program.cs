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
    options.ApiCredentials = new ApiCredentials("<APIKEY>", "<APISECRET>");
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
    return result.Data.SingleOrDefault()?.LastPrice;
})
.WithOpenApi();


app.MapGet("/Balances", async ([FromServices] IBloFinRestClient client) =>
{
    var result = await client.FuturesApi.Account.GetBalancesAsync();
    return (object)(result.Success ? result.Data : result.Error!);
})
.WithOpenApi();

app.Run();