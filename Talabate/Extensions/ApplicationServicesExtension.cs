using Microsoft.AspNetCore.Mvc;
using Talabat.core.Repositories.Contract;
using Talabat.Repositery;
using Talabat.Repositery.Generic_Repository;
using Talabate.Errors;
using Talabate.Helpers;

namespace Talabate.Extensions
{
    public static class ApplicationServicesExtension
    {
public static IServiceCollection AddAplicationServices(this IServiceCollection services)

        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
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

    }
}
