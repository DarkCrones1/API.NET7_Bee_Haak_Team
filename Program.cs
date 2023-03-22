using Microsoft.EntityFrameworkCore;
using Web_API_Kaab_Haak;

var builder = WebApplication.CreateBuilder(args);

var StartUp = new StartUp(builder.Configuration);

StartUp.ConfigureServices(builder.Services);

var app = builder.Build();

StartUp.Configure(app, app.Environment);

app.Run();
