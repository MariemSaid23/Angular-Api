using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Talabat.core.Entities;
using Talabat.core.Repositories.Contract;
using Talabat.Repositery;
using Talabat.Repositery.Data;
using Talabate.Errors;
using Talabate.Helpers;
using Talabate.Middlewares;
using Microsoft.Extensions.Logging;
using Talabate.Extensions;

namespace Talabate
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();




            builder.Services.AddSwaggerServices();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //

            builder.Services.AddAplicationServices();
            var app = builder.Build();
            var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbContext = services.GetRequiredService<StoreContext>();
            var logger = loggerFactory.CreateLogger<Program>();

            try
            {
                await _dbContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error has occurred during apply the migration");
            }

            app.Use(async (httpContext, next) =>
            {
                try
                {
                    await next.Invoke(httpContext);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";
                    var response = app.Environment.IsDevelopment() ?
                        new ApiExcecutionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) :
                        new ApiExcecutionResponse((int)HttpStatusCode.InternalServerError);
                    var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    var json = JsonSerializer.Serialize(response, options);
                    await httpContext.Response.WriteAsync(json);
                }
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMidlewares();
               // app.UseDeveloperExceptionPage();
            }


            app.UseAuthorization();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();
            app.Run();
        }
    }
}
