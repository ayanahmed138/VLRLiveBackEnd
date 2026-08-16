using VLRLiveBackEnd.BackgroundServices;
using VLRLiveBackEnd.Cache;
using VLRLiveBackEnd.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<VLRapiService>(client =>
{
    client.BaseAddress = new Uri("http://163.245.192.237:3001");
});
builder.Services.AddHostedService<LiveMatchPollingService>();
builder.Services.AddSingleton<LiveMatchCache>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
