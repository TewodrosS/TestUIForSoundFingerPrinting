using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebSender.Models;

namespace WebSender.Controllers
{
    public class TrackDataController : Controller
    {
        // GET: TrackData
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendFile()
        {
            string directory = @"D:\Temp\";

            HttpPostedFileBase audioFile = Request.Files["audio"];

            if (audioFile != null && audioFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(audioFile.FileName);
                SendFile(fileName, audioFile.InputStream);
            }

            return RedirectToAction("Index");
        }
        private void SendFile(string path, Stream fileStream)
        {
            AudioRequest request = new AudioRequest();

            var byteContent = ReadByte(fileStream);

            request.Content = byteContent;
            request.FileName = Path.GetFileNameWithoutExtension(path);
            request.FileType = Path.GetExtension(path);

            var requestContent = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(requestContent, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServiceUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                // New code:
                HttpResponseMessage response = client.PostAsync("api/trackdata", content).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string stringResult = response.Content.ReadAsStringAsync().Result;

                    if (string.IsNullOrWhiteSpace(stringResult) || stringResult == "null")
                    {
                        //audio not found

                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<TrackData>(stringResult);
                        FormatJson(stringResult);                        
                    }
                }

                else
                {
                    response.StatusCode.ToString();                    
                }
            }
        }

        public static byte[] ReadByte(Stream input)
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

        private static string FormatJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }
}