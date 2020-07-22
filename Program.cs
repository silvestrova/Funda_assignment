using Funda_assignment_Silvestrova.Configuration;
using Funda_assignment_Silvestrova.Presentation;
using Funda_assignment_Silvestrova.Repositories;
using Funda_assignment_Silvestrova.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Funda_assignment_Silvestrova
{
    class Program
    {
        static string defaultCity = "amsterdam";
        static string defaultParameter = "tuin";
        static ServiceProvider serviceProvider;
        static void Main(string[] args)
        {
            //setup our DI
            serviceProvider = ConfigureServices();

            var logger = serviceProvider.GetService<ILogger<Program>>();

            var agentsRepository = serviceProvider.GetService<IAgentRepository>();
            CalculateTable(agentsRepository, 
                new string[] { defaultCity });
            CalculateTable(agentsRepository, new string[] { defaultCity, defaultParameter });

            logger.LogDebug("All done!");
        }
        static void CalculateTable(IAgentRepository agentRepository,string [] parameters)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var agents = agentRepository.GetOrderedAgents(parameters).Take(10);
            Console.Clear();
            Console.WriteLine("agents table for Amsterdam");
            ConsoleDisplay.PrintLine();
            ConsoleDisplay.PrintRow("Agent Listings Amount", "Agent Id:Name");
            ConsoleDisplay.PrintLine();
            foreach (var agent in agents)
                ConsoleDisplay.PrintRow(agent.ListingsAmount.ToString(), agent.GetAgentDescription());
            ConsoleDisplay.PrintLine();
            stopwatch.Stop();
            Console.WriteLine($"took {stopwatch.ElapsedMilliseconds} ms to execute");

            Console.ReadLine();
        }
        static ServiceProvider ConfigureServices()
        {
          var configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
         .AddJsonFile("appsettings.json", false)
         .Build();

            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.AddConsole(options => options.IncludeScopes = true);
            });
            services.AddSingleton<IConfiguration>(configuration);
            services.AddHttpClient<IListingService, ListingService>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("RequestData:baseUrl"));
            });
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IListingRepository, ListingRepository>();
            services.AddSingleton<IConfigurationHelper, ConfigurationHelper>();

            return services.BuildServiceProvider();
        }
    }
}
