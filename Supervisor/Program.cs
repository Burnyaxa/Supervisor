using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;
using Supervisor.Interfaces;
using Supervisor.Jobs;
using Supervisor.Services;

namespace Supervisor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<DreamContext>();
                    services.AddSingleton<IWordsService, WordsService>();
                    JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                  
                    services.AddQuartz(q =>  
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();
                        var jobKey = new JobKey("SendWordsJob");
                        q.AddJob<SendWordsJob>(o => o.WithIdentity(jobKey));
                        q.AddTrigger(o => o
                            .ForJob(jobKey)
                            .WithIdentity("trigger")
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever()));
                    });
                    

                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);
                    
                });
    }
}