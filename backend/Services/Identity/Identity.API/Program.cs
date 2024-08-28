using Auth;
using Identity.API.DI;
using Identity.Application.DI;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureSqlServerContext(builder.Configuration)
    .ConfigureIdentity()
    .ConfigureIdentityServer(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureApi()
    .AddEndpointsApiExplorer();

builder.Services.AddControllers()
    .AddApplication();

builder.Services.AddBearerAuth("https://localhost:7068");
builder.Services.AddSwaggerGen("https://localhost:7068");

var app = await builder.Build()
    .MigrateDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger UI");
        opt.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        opt.OAuthClientId("client_id_swagger");
        opt.OAuthClientSecret("client_secret_swagger");
    });
}
app.UseCors("CorsPolicy");

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseIdentityServer();

app.Run();
