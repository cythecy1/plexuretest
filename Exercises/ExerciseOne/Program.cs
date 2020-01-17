using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExerciseOne
{

    /// <summary>
    /// Gets data from 3 resources
    /// Print Sumn Content Length.
    /// Any args will Cancel Task.
    /// </summary>
    class Program
    {
        private string[] _resources = new string[] {
            "https://dog.ceo/api/breeds/list/all",
            "https://dog.ceo/api/breed/hound/images",
            "https://dog.ceo/api/breed/hound/list"
        };

        /// <param name="resources">Resources can be set to null to use private resources</param>
        public Program(string[] resources)
        {
            if(resources?.Length > 0)
            {
                _resources = resources;
            }
        }


        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            if (args.Length > 0)
                cts.Cancel();

            Program program = new Program(null);
            var taskResult = program.GetTotalContentLength(cts.Token);
            Console.WriteLine($"Total = {taskResult.Result}");
        }



        public async Task<int> GetTotalContentLength(CancellationToken token)
        {
            using (var client = new HttpClient())
            {
                var taskResult = await Task.WhenAll(_resources.Select(async resource => {
                    int lengthResult = 0;
                    try
                    {
                        lengthResult = await GetResourceStringLength(client, resource, token);
                        
                    }
                    catch(TaskCanceledException)
                    {
                        Console.WriteLine($"GetResourceStringLength Cancelled: URL - {resource}");
                    }
                    return lengthResult;
                }));
                return taskResult.Sum();
            }
        }

        private async Task<int> GetResourceStringLength(HttpClient httpClient, string resource, CancellationToken token)
        {
            var response = await httpClient.GetAsync(resource, token);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent.Length;
        }
    }
}
