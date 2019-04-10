using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MySpeechApp
{
    class Program
    {
        // https://www.w3.org/TR/speech-synthesis/
        private const string TOKEN_URI = "https://southeastasia.api.cognitive.microsoft.com/sts/v1.0/issuetoken";
        private const string SUB_KEY = "36290bbded8f4cb59e34e50ed7be60b0";
        private const string HOST = "https://southeastasia.tts.speech.microsoft.com/cognitiveservices/v1";
        private const string RESOURCE_NAME = "MySpeechService";

        static void Main(string[] args)
        {
            var result = GetTokenAsync().ConfigureAwait(false).GetAwaiter();
            string token = result.GetResult();

            var text1 = "你好，我是来自博客园的技术爱好者 Ron Liang；很高兴可以试用 Speech，希望一切顺利。";
            var task1 = RequestSSML(token, text1, "1.wav");
            task1.ConfigureAwait(false).GetAwaiter().GetResult();

            var text2 = "小哥哥，来一发<prosody rate=\"-40.00%\" volume=\"-80.00%\" duration=\"1.5s\">吗？</prosody>";
            var task2 = RequestSSML(token, text2, "2.wav");
            task2.ConfigureAwait(false).GetAwaiter().GetResult();

            var text3 = "蒿嗨偶，肝绝忍僧衣襟捣打的高草，肝绝忍僧衣襟捣打了巅峰。蒿赠寒，蒿朵母，蒿悬猜。";
            var task3 = RequestSSML(token, text3, "3.wav");
            task3.ConfigureAwait(false).GetAwaiter().GetResult();

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }

        private static async Task<string> GetTokenAsync()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SUB_KEY);
                var builder = new UriBuilder(TOKEN_URI);

                var result = await httpClient.PostAsync(builder.Uri.AbsoluteUri, null);

                return await result.Content.ReadAsStringAsync();
            }
        }

        private static async Task RequestSSML(string authToken, string text, string fileName)
        {
            Console.WriteLine("准备中...");
            using (var httpClient = new HttpClient())
            {
                var body = "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\" version=\"1.0\" xml:lang=\"zh-CN\"><voice name=\"Microsoft Server Speech Text to Speech Voice (zh-CN, XiaoxiaoNeural)\">" + text + "</voice></speak>";
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(HOST),
                    Content = new StringContent(body, Encoding.UTF8, "application/ssml+xml")
                };
                request.Headers.Add("Authorization", "Bearer " + authToken);
                request.Headers.Add("Connection", "Keep-Alive");
                request.Headers.Add("User-Agent", RESOURCE_NAME);
                request.Headers.Add("X-Microsoft-OutputFormat", "riff-24khz-16bit-mono-pcm");

                Console.WriteLine("正在进行远程过程调用...");

                var response = await httpClient.SendAsync(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("The Response {0}", response.StatusCode);
                    return;
                }
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    stream.Position = 0;
                    Console.WriteLine("正在下载语音文件 {0} ...", fileName);
                    using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        await stream.CopyToAsync(fs);
                        fs.Close();
                    }
                }
                Console.WriteLine("文本转换语音成功");
                Console.WriteLine("===============\n");
            }
        }
    }
}
