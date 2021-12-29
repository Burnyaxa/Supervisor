using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using RestSharp;
using Supervisor.Interfaces;
using Supervisor.RequestModels;

namespace Supervisor.Jobs
{
    [DisallowConcurrentExecution]
    public class SendWordsJob : IJob
    {
        private readonly string _urlBase = Environment.GetEnvironmentVariable("worker-pool-load-balancer", EnvironmentVariableTarget.User);
        private readonly ILogger<SendWordsJob> _logger;
        private readonly IWordsService _service;
        private static readonly List<Keyword> UsedWords = new List<Keyword>();

        public SendWordsJob(ILogger<SendWordsJob> logger, IWordsService service)
        {
            _logger = logger;
            _service = service;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            var words = _service.GetWords().ToList();
            var wordsToSend = words.Where(x => UsedWords.All(y => x.Id != y.Id)).ToList();
            var wordsToDelete = UsedWords.Where(x => words.All(y => y.Id != x.Id)).ToList();
            _logger.LogInformation($"Got {words.Count} words. Words to send: {wordsToSend.Count} Words to delete {wordsToDelete.Count}");

            var client = new RestClient();
            foreach (var word in wordsToSend)
            {
                var request = new RestRequest(_urlBase, Method.Post);
                var body = new CreateWorkerModel()
                {
                    Keyword = word.Keyword1,
                    KeywordId = word.Id,
                    Frequency = 20,
                    NumberOfPages = 1
                };
                
                request.AddBody(body);
                await client.ExecuteAsync(request);
                
                UsedWords.Add(word);
            }

            foreach (var word in wordsToDelete)
            {
                var request = new RestRequest(_urlBase, Method.Delete);
                request.AddParameter("id", word.Id);
                await client.ExecuteAsync(request);
                UsedWords.Remove(word);
            }
        }
    }
}