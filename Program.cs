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

        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = ConfigureServices();

            var logger = serviceProvider.GetService<ILogger<Program>>();

            var agentsRepository = serviceProvider.GetService<IAgentRepository>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var agents = agentsRepository.GetOrderedAgents(new string[]{defaultCity}).Take(10);
            Console.Clear();
            Console.WriteLine("agents table for Amsterdam");
            ConsoleDisplay.PrintLine();
            ConsoleDisplay.PrintRow("Agent Listings Amount", "Agent Id:Name");
            ConsoleDisplay.PrintLine();
            foreach (var agent in agents)
                ConsoleDisplay.PrintRow(agent.ListingsAmount.ToString(), agent.GetAgentDescription());
            ConsoleDisplay.PrintLine();
            stopwatch.Stop();
            Console.WriteLine($"took {stopwatch.ElapsedMilliseconds} ms");
          
            Console.ReadLine();
            
            stopwatch.Restart();
            agents = agentsRepository.GetOrderedAgents(new string[] { defaultCity, defaultParameter }).Take(10);
            Console.Clear();
            Console.WriteLine("agents table for Amsterdam/Tuin");
            ConsoleDisplay.PrintLine();
            ConsoleDisplay.PrintRow("Agent Listings Amount", "Agent Id:Name");
            ConsoleDisplay.PrintLine();
            foreach (var agent in agents)
                ConsoleDisplay.PrintRow(agent.ListingsAmount.ToString(), agent.GetAgentDescription());
            ConsoleDisplay.PrintLine();
            stopwatch.Stop();
            Console.WriteLine($"took {stopwatch.ElapsedMilliseconds} ms");
            Console.ReadLine();
           
            //do the actual work here


            logger.LogDebug("All done!");


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
