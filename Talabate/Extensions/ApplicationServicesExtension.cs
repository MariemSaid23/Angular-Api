using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.core;
using Talabat.core.Repositories.Contract;
using Talabat.core.Services.Contract;
using Talabat.Repositery;
using Talabat.Repositery.Basket_Repository;
using Talabat.Repositery.Generic_Repository;
using Talabat.services.AuthService;
using Talabate.Errors;
using Talabate.Helpers;

namespace Talabate.Extensions
{
    public static class ApplicationServicesExtension
    {
public static IServiceCollection AddAplicationServices(this IServiceCollection services)

        {
            services.AddScoped(typeof(IunitOfWork), typeof(UnitOfWork));

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (ActionContext =>
                {
                    var errors = ActionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                        .SelectMany(P => P.Value.Errors)
                        .Select(E => E.ErrorMessage)
                        .ToArray();
                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                });
            });
            return services;

        }



        public static IServiceCollection AddAuthServices(this IServiceCollection services,IConfiguration configuration)
        {
           services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidIssuer = configuration["JWT:ValidIussuer"],
                       ValidateAudience = true,
                       ValidAudience = configuration["JWT:ValidAudience"],
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,


                   };
               });

            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            return services;
        }
    }


}
