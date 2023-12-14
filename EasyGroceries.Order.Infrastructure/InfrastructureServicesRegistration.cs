using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Infrastructure.Contracts;
using EasyGroceries.Order.Infrastructure.DBHandler;
using EasyGroceries.Order.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
            services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
            services.AddScoped<IDapper, DatabaseHandler>();
            return services;
        }
    }
}
