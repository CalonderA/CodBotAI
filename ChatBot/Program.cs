using ChatBot;
using ChatBot.Commands;
using ChatBot.Repositories.Implementations;
using ChatBot.Repositories.Interfaces;
using ChatBot.Settings;
using Serilog;
using Telegram.Bot;

public class Program
{
    public static void Main(string[] args)
    {
        // Настройка Serilog: логи в консоль и в файл
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
            .CreateBootstrapLogger();

        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog();

        // Add services to the container.

        builder.Services.AddControllers().AddNewtonsoftJson();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<ChatApiSettings>(
            builder.Configuration.GetSection("ChatApi"));

        builder.Services.Configure<TelegramSettings>(
            builder.Configuration.GetSection("Telegram"));

        builder.Services.AddSingleton<ITelegramBotClient>(sp =>
        {
            var token = builder.Configuration["Telegram:BotToken"];
            return new TelegramBotClient(token!);
        });

        builder.Services.AddSingleton<IBotCommand, StartCommand>();
        builder.Services.AddSingleton<IBotCommand, HelpCommand>();
        builder.Services.AddSingleton<IBotCommand, StatsCommand>();
        builder.Services.AddSingleton<IBotCommand, ClearCommand>();
        builder.Services.AddSingleton<IBotCommand, UndoCommand>();
        builder.Services.AddSingleton<IBotCommand, SummarizeCommand>();
        builder.Services.AddSingleton<IBotCommand, JokeCommand>();
        builder.Services.AddSingleton<TelegramUpdateProcessor>();
        builder.Services.AddHttpClient<IChatApiClient, HttpChatApiClient>();
        builder.Services.AddSingleton<IChatModelRepository, ChatModelRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}