using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
namespace GPTDemoTest
{
    internal class Program
    {
        private static readonly HttpClient _client = new HttpClient();
        public static string apiKey = "ApiKeyHeree";
        public static string endpointURL = "https://api.openai.com/v1/completions";
        public static string modelType = "gpt-3.5-turbo-instruct";
        public static int maxTokens = 256;
        public static double temperature = 1.0f;
        static async Task Main(string[] args)
        {
            await RunChatGPT();
        }

        public static async Task RunChatGPT()
        {
            Console.WriteLine("Enter your question");
            string prompt = Console.ReadLine();
            await Console.Out.WriteLineAsync("Thinking...");

            string response = await InteractionGPT(apiKey, endpointURL, modelType, prompt,  maxTokens, temperature);


            textCompletionResponse deserializedResponse = JsonConvert.DeserializeObject<textCompletionResponse>(response);
            string promptResponse = deserializedResponse.Choices[0].Text;
            await Console.Out.WriteLineAsync("response:");
            await Console.Out.WriteLineAsync(promptResponse);
            await Console.Out.WriteLineAsync("___________________________");

            await RunChatGPT();
        }

        public static async Task<string> InteractionGPT(string apiKey, string endpoint, string modeltype, string promptt, int maxtokens, double temp)
        {
            var resquestbody = new
            {
                model = modeltype,
                prompt = promptt,
                max_tokens = maxtokens,
                temperature = temp
            };

            string jsonRequest = JsonConvert.SerializeObject(resquestbody);

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var httpResponse = await _client.SendAsync(request);
            string responseContent = await httpResponse.Content.ReadAsStringAsync();

            return responseContent;
        }

        public class Choice
        {
            public string Text { get; set; }
        }

        public class textCompletionResponse
        {
            public Choice[] Choices { get; set; }
        }
    }
}
