
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using Ultimate_ASP.Net_Core_Web_API.Extensions;

namespace Ultimate_ASP.Net_Core_Web_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            // Add services to the container.
            builder.Services.ConfigureCors();
            builder.Services.ConfigureIISIntegration();
            builder.Services.ConfigureLoggerService();
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureSQLContext(builder.Configuration);


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
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });
            app.UseCors("CorsPolicy");

            app.UseAuthorization();
            // app.Run(async context => { await context.Response.WriteAsync("Hello from the middleware component."); });
            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine($"Logic before executing the next delegate in the Use method");
            //    await next.Invoke();
            //    Console.WriteLine($"Logic after executing the next delegate in the Use method");
            //});
            //app.Run(async context =>
            //{
            //    Console.WriteLine($"Writing the response to the client in the Run method");
            //    await context.Response.WriteAsync("Hello from the middleware component.");
            //});

            app.MapControllers();

            app.Run();
        }
    }
}
