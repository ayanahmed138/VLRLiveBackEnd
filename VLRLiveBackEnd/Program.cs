using VLRLiveBackEnd.BackgroundServices;
using VLRLiveBackEnd.Cache;
using VLRLiveBackEnd.Data;
using VLRLiveBackEnd.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<VLRapiService>(client =>
{
   // client.BaseAddress = new Uri("http://163.245.192.237:3001");
    client.BaseAddress = new Uri("http://127.0.0.1:3001");
    //client.BaseAddress = new Uri("http://192.168.0.104:3001");
});

builder.Services.AddHttpClient<TeamLogoService>();
builder.Services.AddHostedService<LiveMatchPollingService>();
builder.Services.AddSingleton<LiveMatchCache>();
builder.Services.AddScoped<TeamSyncService>();
builder.Services.AddScoped<TeamIconResolver>();
builder.Services.AddSingleton<TeamSyncQueue>();
builder.Services.AddHostedService<TeamAutoSyncService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.Run();
