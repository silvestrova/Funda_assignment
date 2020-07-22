using Funda_assignment_Silvestrova.Configuration;
using Funda_assignment_Silvestrova.Presentation;
using Funda_assignment_Silvestrova.Repositories;
using Funda_assignment_Silvestrova.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
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
            CalculateTable(new string[] { defaultCity });
            CalculateTable(new string[] { defaultCity, defaultParameter });

            logger.LogDebug("All done!");
        }
        static void CalculateTable(string [] parameters)
        {
            Stopwatch stopwatch = new Stopwatch();
            var agentsRepository = serviceProvider.GetService<IAgentRepository>();
            stopwatch.Start();
            var agents = agentsRepository.GetOrderedAgents(parameters).Take(10);
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
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.AddConsole(options => options.IncludeScopes = true);
            });
            services.AddHttpClient<IListingService, ListingService>(client =>
            {
                client.BaseAddress = new Uri("http://partnerapi.funda.nl/");
            });
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IListingRepository, ListingRepository>
                (x=>new ListingRepository(x.GetRequiredService<IListingService>(), x.GetRequiredService<ILogger<ListingRepository>>(), 100));
            services.AddSingleton<IConfigurationHelper, ConfigurationHelper>(x => new ConfigurationHelper("ac1b0b1572524640a0ecc54de453ea9f"));

            return services.BuildServiceProvider();
        }
    }
}
