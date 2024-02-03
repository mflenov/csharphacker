using System;
using System.Net;
using System.Text;

namespace Network
{
	public class WebRequestClient
	{
        static readonly HttpClient client = new HttpClient();

        public async Task<string> Execute()
		{
            try
            {
                using (HttpResponseMessage response = await client.GetAsync("http://www.flenov.info/"))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}

