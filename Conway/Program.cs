// See https://aka.ms/new-console-template for more information

using Conway;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IConsoleFacade, ConsoleFacade>();

builder.Services.AddSingleton<IAction, InputGridSizeAction>();
builder.Services.AddSingleton<IAction, InputNumberOfGenerationAction>();
builder.Services.AddSingleton<IAction, InputLiveCellAction>();
builder.Services.AddSingleton<IAction, PrintGameStateAction>();
builder.Services.AddSingleton<IAction, RunAction>();
builder.Services.AddSingleton<IAction, TerminateAction>();

builder.Services.AddSingleton<IFreshGameStateFactory, FreshGameStateFactory>();
builder.Services.AddSingleton<IDisplayMenuActionFactory, DisplayMenuActionFactory>();
builder.Services.AddSingleton<IGameController, GameController>();

using var host = builder.Build();

var gameController = host.Services.GetRequiredService<IGameController>();
gameController.Play();