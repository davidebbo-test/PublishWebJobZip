using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PublishWebJobZip
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Syntax: PublishWebJobZip.exe MyJobFiles.zip https://mysite.scm.azurewebsites.net/api/continuouswebjobs/myjob UserName Password");
                Environment.Exit(1);
            }

            string zipFile = args[0];
            string url = args[1];
            string userId = args[2];
            string password = args[3];

            using (HttpClient client = new HttpClient())
            {
                var fileContent = new StreamContent(File.OpenRead(zipFile));

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = Path.GetFileName(zipFile)
                };

                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                var byteArray = Encoding.ASCII.GetBytes(new StringBuilder(userId).Append(":").Append(password).ToString());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                try
                {
                    var response = client.PutAsync(url, fileContent);
                    Console.WriteLine(response.Result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
