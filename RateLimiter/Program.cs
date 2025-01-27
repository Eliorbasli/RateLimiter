
using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static async Task Main(string[] args)
    {

        // Example free API 
        object argument = "https://restcountries.com/v3.1/region/europe";
        

        RateLimiter rate1 = new RateLimiter(SendGetRequestAsync , 3 , RateLimiter.MINUTE);
        

        var threads = new List<Thread>();
        for( int i = 0 ; i < 3 ; i++)
        {
            var thread = new Thread(() => Worker(rate1 , argument));
            threads.Add(thread);
            thread.Start();
        }

        foreach (var thread in threads){
            thread.Join();
        }
    }

/////////////////////////////////////////////////////////////////////
        
    static void Worker(RateLimiter rate1, object argument)
        {
            for (int i = 0 ; i < 5 ; i++)
            {
                rate1.Perform(argument).Wait();
            }
        }
    static async Task SendGetRequestAsync(object argument)
    {
        if (argument is string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    Console.WriteLine($"[{DateTime.Now}] The Status code from {url}: {response.StatusCode}");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid argument type.");
        }
    }
}