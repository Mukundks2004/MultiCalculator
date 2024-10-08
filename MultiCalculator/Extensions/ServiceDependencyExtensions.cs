﻿using Microsoft.Extensions.DependencyInjection;
using MultiCalculator.Database;
using MultiCalculator.Database.Models;
using MultiCalculator.Database.Repositories;
using MultiCalculator.Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace MultiCalculator.Extensions
{
    public static class ServiceDependencyExtensions
    {
        public static IServiceCollection AddDependencyGroup(
            this IServiceCollection services)
        {
            services.AddDbContext<CalculatorDbContext>()
                .AddScoped<CalculationHistoryRepository>()
                .AddScoped<UserRepository>()
                .AddScoped<OpenAiQuestionsRepository>()
                .AddScoped<ChatBotHistoryRepository>()
                .AddTransient<IDatabaseService, DatabaseService>();

            return services;
        }
    }
}