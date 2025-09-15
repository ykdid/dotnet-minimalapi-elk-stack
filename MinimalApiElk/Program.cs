using Elasticsearch.Net;
using MinimalApiElk.Models;
using Nest;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://+:80");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Build().UseCors("AllowAll");

var elasticUrl = builder.Configuration["ElasticConfiguration:Uri"] ?? "http://localhost:9200";
var defaultIndex = builder.Configuration["ElasticConfiguration:DefaultIndex"] ?? "logs";

var settings = new ConnectionSettings(new Uri(elasticUrl))
    .DefaultMappingFor<LogData>(m => m
        .PropertyName(p => p.Timestamp, "@timestamp")
    );

var client = new ElasticClient(settings);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUrl))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"app-logs-{DateTime.UtcNow:yyyy-MM}"
    })
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.MapPost("/api/logs", async (LogData logData) =>
{
    try
    {
        var indexName = $"api-{defaultIndex}-{DateTime.UtcNow:yyyy-MM}";
        
        var response = await client.IndexAsync(logData, i => i
            .Index(indexName)
        );
        
        if (response.IsValid)
        {
            Log.Information("Log data successfully indexed to Elasticsearch in index {IndexName}", indexName);
            return Results.Ok(new { 
                message = "Log data successfully stored", 
                id = response.Id,
                index = response.Index,
                attempted_index = indexName
            });
        }
        
        Log.Error("Failed to index log data: {ErrorMessage}", response.DebugInformation);
        return Results.BadRequest(new { message = "Failed to store log data", error = response.DebugInformation });
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error occurred while storing log data");
        return Results.StatusCode(500);
    }
});

app.Run();