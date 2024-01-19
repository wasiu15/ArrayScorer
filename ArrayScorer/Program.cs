using ArrayScorer.Exceptions;
using ArrayScorer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArrayScorer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddLogging();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Register custom services
            builder.Services.AddScoped<IGetTotalScoreService, GetTotalScoreService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            // Add the custom global exception handler middleware
            app.UseMiddleware<GlobalExceptionHandler>();


            app.UseAuthorization();

            // Add the custom global exception handler middleware
            //app.ConfigureExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}