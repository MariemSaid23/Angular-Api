using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net;
using System.Text;
using System.Text.Json;
using Talabat.core.Entities.Identity;
using Talabat.core.Repositories.Contract;
using Talabat.core.Services.Contract;
using Talabat.Repositery.Basket_Repository;
using Talabat.Repositery.Data;
using Talabat.Repositery.Identity;
using Talabat.services.AuthService;
using Talabate.Errors;
using Talabate.Extensions;

namespace Talabate
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string txt = "hi";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddNewtonsoftJson(option=>{
            option.SerializerSettings.ReferenceLoopHandling=ReferenceLoopHandling.Ignore;
            });


            builder.Services.AddScoped<IConnectionMultiplexer>((ServiceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            builder.Services.AddSwaggerServices();
            builder.Services.AddAplicationServices();


            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {

                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {

            }).AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            //builder.Services.AddScoped(typeof(IAuthService),typeof(IAuthService));
           builder.Services.AddAuthServices(builder.Configuration);
            //cors
            builder.Services.AddCors(options => {
                options.AddPolicy(txt,
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });

            });
            var app = builder.Build();
            var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbContext = services.GetRequiredService<StoreContext>();
            var _identitydbContext = services.GetRequiredService<ApplicationIdentityDbContext>();

            var logger = loggerFactory.CreateLogger<Program>();

            try
            {
                await _dbContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_dbContext);
                await _identitydbContext.Database.MigrateAsync();

                var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                await ApplicationIdentityDbContextSeed.SeedUserAsync(_userManager );
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
                    var json = System.Text.Json.JsonSerializer.Serialize(response, options);
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
            app.UseCors(txt);
            app.MapControllers();
            app.Run();
        }
    }
}
