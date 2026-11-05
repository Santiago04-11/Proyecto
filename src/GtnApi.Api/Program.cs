using GtnApi.Api.Endpoints;
using GtnApi.Core;
using GtnApi.Core.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGtnCore();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<GtnDbContext>>();
    await DatabaseInitializer.MigrateAsync(factory);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapNotesEndpoints();
app.MapTasksEndpoints();
app.MapProjectsEndpoints();

app.Run();
