using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JokeGenerator
{
    class RandomNameService
    {
        private const string name_api_url = "https://www.names.privserv.com/api/";

		/// <summary>
		/// returns an Person object that contains name,gender ,region and surname
		/// </summary>
		/// <param name="client2"></param>
		/// <returns></returns>
		public static async Task<Person> Getnames()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					client.BaseAddress = new Uri(name_api_url);
					var result = await client.GetStringAsync("");
					return JsonConvert.DeserializeObject<Person>(result);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An Error Occured:  " + ex.Message);
				return null;
			}
		}
	}
}
