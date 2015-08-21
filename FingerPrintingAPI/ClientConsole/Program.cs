using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;


namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {                                  
            string url = "http://localhost:7375/";
            string path = @"D:\GraduationMusic\ATsednya\Tsedenia GM - Atalay.mp3";

            MemoryStream requestStream = new MemoryStream();
            Stream fileStream = File.Open(path, FileMode.Open);

            var byteFile = ReadFully(fileStream);

            var base64Doc = Convert.ToBase64String(byteFile);

            HttpContent content = new ByteArrayContent(byteFile);
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                
                // New code:
                HttpResponseMessage response = client.PostAsync("api/trackdata/file", content).Result;

                var result = response.Content;
                
            }            
        }


        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
   
}
