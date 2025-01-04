using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using TextToSQL.Commands;
using TextToSQL.Data;
using TextToSQL.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
#pragma warning disable SKEXP0010
builder.Services.AddSemanticServices();
#pragma warning restore SKEXP0010

builder.Services.AddDbContext<AppDbContext>(cfg =>
    cfg.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("chat", async (IMediator mediator, ChatCommand command) => Results.Ok( await mediator.Send(command)));

app.Run();