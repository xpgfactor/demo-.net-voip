using ListenerService.Consumers;
using SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ListenerHub>();


builder.Services.AddSignalR(hubOption => hubOption.MaximumReceiveMessageSize = 100_100);
builder.Services.AddHostedService<Consumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.WithOrigins("http://localhost:4200", "http://localhost:62476");
    x.AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ListenerHub>("/listener", options => {
    options.TransportMaxBufferSize = 0;
    options.ApplicationMaxBufferSize = 0;
    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(10);
    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(10);
    options.TransportSendTimeout = TimeSpan.FromSeconds(10);
});

app.Run();
