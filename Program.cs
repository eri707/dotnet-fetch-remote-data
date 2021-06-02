using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace dotnet_fetch_remote_data
{
    class Program
    {//Task, for an async method that performs an operation but returns no value.
        static async Task Main(string[] args)
        {//HttpClient is a base class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI
            using (var client = new HttpClient())
            { //Gets or sets the base address of Uniform Resource Identifier (URI) of the Internet resource used when sending requests.
                client.BaseAddress = new Uri("https://restcountries.eu");
                //Send a GET request to the specified Uri as an asynchronous operation.
                var response = await client.GetAsync("rest/v2/all?fields=name;population");
                // Throw an error message just in case the server doesn't respond 
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("There was an error fetching data.");
                    return;
                }
                // ReadAsStringAsync means to serialize the HTTP content to a string as an asynchronous operation
                // This is the json string from api
                var content = await response.Content.ReadAsStringAsync();
                var countryList = JsonSerializer.Deserialize<List<Country>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var heighestPopulationCountry = countryList.OrderByDescending(i => i.Population).First();
                Console.WriteLine($"The highest population country {heighestPopulationCountry.Name}: {heighestPopulationCountry.Population}");
            }
        }
    }
}
