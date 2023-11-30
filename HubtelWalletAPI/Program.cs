using HubtelWalletAPI.Authentication;
using HubtelWalletAPI.Swagger;
using HubtelWalletDatabase.DataAccess;
using HubtelWalletDatabase.DatabaseAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.ExtendedSwaggerGen();
builder.Services.AddScoped<WalletDataAccess>();
builder.Services.AddScoped<WalletDatabaseMock>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseMiddleware<BasicAuthenticationHandler>("Test");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
