using Microsoft.EntityFrameworkCore;
using Talabat.Repositery.Data;

namespace Talabate
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options=>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            var app = builder.Build();




            //try {
            //    var services = scope.ServiceProvider;
            //    var _dbContext = services.GetRequiredService<StoreContext>();
            //    //Ask clr for Creating Dbcontext Explicitly
            //    await _dbContext.Database.MigrateAsync();
            //}
            //finally { scope.Dispose(); }
            //بدل try finally هستخدم keyWord Using do Dispose all scope
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbContext = services.GetRequiredService<StoreContext>();
            //Ask clr for Creating Dbcontext Explicitly
            var loggerFactory= services.GetRequiredService<ILoggerFactory>();   
            try
            {
                await _dbContext.Database.MigrateAsync();//update-DataBase
                await StoreContextSeed.SeedAsync(_dbContext);//Data seeding
            }
            catch (Exception ex)
            {
                var logger=loggerFactory.CreateLogger<Program>();
                logger.LogError(ex,"an error has been accured during apply the migration");
                
               
            } 

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
