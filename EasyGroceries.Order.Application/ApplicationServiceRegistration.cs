using EasyGroceries.Order.Application.Contracts.MessageBus;
using EasyGroceries.Order.Application.Contracts.Services;
using EasyGroceries.Order.Application.Services;
using EasyGroceries.Order.Application.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("Product", u => u.BaseAddress = new Uri(configuration["ServiceUrls:ProductAPI"]));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IMessageBus, MessageBus>();
            services.AddValidatorsFromAssemblyContaining<OrderHeaderDtoValidator>();
            return services;
        }
    }
}
