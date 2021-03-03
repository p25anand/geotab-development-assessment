using JokeGenerator;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp1
{
    class RandomJokeService
    {
        private const string _url = "https://api.chucknorris.io";
        private const string categories_url = "jokes/categories";
        private const string random_jokes_url = "jokes/random";


        public static async Task<string> GetRandomJokes(Person person, string category)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //modify url to get categories
                    client.BaseAddress = new Uri(_url);
                    var uriBuilder = new UriBuilder(client.BaseAddress + random_jokes_url);
                    var query_param = HttpUtility.ParseQueryString(uriBuilder.Query);

                    if (category != null)
                    {
                        query_param["category"] = category;

                    }
                    uriBuilder.Query = query_param.ToString();

                    //get joke
                    string joke = await client.GetStringAsync(random_jokes_url);
                    //replace chuck norris name with random name
                    if (person.Name != null && person.Surname != null)
                    {
                        string oldname = @"\bChuck Norris\b";
                        string newname = person.Name + " " + person.Surname;
                        joke = Regex.Replace(joke, oldname, newname);

                    }

                    return JsonConvert.DeserializeObject<dynamic>(joke).value;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("An Error Occured:  " + ex.Message);
                return null;
            }
        }

        //get categories
        public static async Task<string[]> GetCategories()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_url);
                    string Category_json = await client.GetStringAsync(categories_url);
                    return JsonConvert.DeserializeObject<string[]>(Category_json);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;

            }

        }
    }
}
