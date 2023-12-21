using doviz_bot.Brokers.Storages;
using doviz_bot.Brokers.Telegrams;
using doviz_bot.Services.Foundations.Converters;
using doviz_bot.Services.Foundations.Telegrams;
using doviz_bot.Services.Foundations.TelegramUsers;
using doviz_bot.Services.Orchestrations.Telegrams;
using doviz_bot.Services.Processings.Telegrams;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add your services using the provided methods
AddBrokers(builder);
AddFoundationServices(builder);
AddProcessingServices(builder);
AddOrchestrationServices(builder);

var app = builder.Build();
RegisterEventListeners(app);

app.UseSwagger();
app.UseSwaggerUI();

static void AddBrokers(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<IStorageBroker, StorageBroker>();
    builder.Services.AddSingleton<ITelegramBroker, TelegramBroker>();
}

static void AddFoundationServices(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<ITelegramService, TelegramService>();
    builder.Services.AddTransient<ITelegramUserService, TelegramUserService>();
    builder.Services.AddTransient<IConverterService, ConverterService>();
}

static void AddProcessingServices(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<ITelegramUserProcessingService, TelegramUserProcessingService>();

}
static void AddOrchestrationServices(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<ITelegramUserOrchestrationService, TelegramUserOrchestrationService>();
}

static void RegisterEventListeners(IApplicationBuilder app)
{
    app.ApplicationServices.GetRequiredService<ITelegramUserOrchestrationService>()
                .ListenTelegramUserMessage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
