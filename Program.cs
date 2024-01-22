using CustomerAPI.DataContext;
using CustomerAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CustomerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //Add typed client JsonTypicode
            builder.Services.AddHttpClient<JsonTypicodeServices>(opt => {
                opt.BaseAddress = new Uri(builder.Configuration.GetSection("JsonTipecode").GetSection("baseUrl").Value);
                opt.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            // Register DataContext
            builder.Services.AddDbContext<AppDataContext>();

            // Add SeriLog
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
        }
    }
}
