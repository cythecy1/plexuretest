using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExerciseOne
{

    /// <summary>
    /// Gets data from 3 resources
    /// 1.  https://dog.ceo/api/breeds/list/all
    /// 2.  https://dog.ceo/api/breed/hound/images
    /// 3.  https://dog.ceo/api/breed/hound/list
    /// 
    /// Print Total Content.length.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            Program p = new Program();
            p.ParallelThis();
            Console.WriteLine("Hello World!");
        }

        public async Task<int> GetAllDogBreeds()
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetAllHoundImages()
        {
            throw new IndexOutOfRangeException();
        }

        public async Task<int> GetAllHoundSubBreed()
        {
            throw new NotImplementedException();
        }

        public void ParallelThis()
        {
            List<int> results = new List<int>();
            List<int> myRange = Enumerable.Range(1, 10).ToList();
            Parallel.ForEach(myRange, async act =>
            {
                //var res = await new HttpClient().GetAsync("https://dog.ceo/api/breeds/list/all");
                Console.WriteLine(act);
                results.Add(act);
            });

            Parallel.For(0, myRange.Count, index => {
                //var res = await new HttpClient().GetAsync("https://dog.ceo/api/breeds/list/all");
                Console.WriteLine(index);
            });

            /*
            int totalDistanceTravel;
            Parallel.For(0, itinerary.Waypoints.Count, async index => { 
                var distance = await _distanceCalculator.GetDistanceAsync(itinerary.Waypoints[index])
            });
            {
                result = result + _distanceCalculator.GetDistanceAsync(itinerary.Waypoints[i],
                     itinerary.Waypoints[i + 1]).Result;
            }
            **/



        }



    }
}
