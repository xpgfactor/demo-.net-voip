using Confluent.Kafka;
using Kafka.Producers;
using MessageService;
using SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IKafkaProducer<Null, string>, SimpleStringProducer>();
builder.Services.AddSingleton<StreamHub>();

builder.Services.AddSignalR(hubOption=>hubOption.MaximumReceiveMessageSize=100_100);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.WithOrigins("", "");
    x.AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<StreamHub>("/streamHub", options => {
    options.TransportMaxBufferSize = 0;
    options.ApplicationMaxBufferSize = 0;
    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(10);
    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(10);
    options.TransportSendTimeout = TimeSpan.FromSeconds(10);
});

app.MapHub<ChatHub>("/chat");

app.Run();
