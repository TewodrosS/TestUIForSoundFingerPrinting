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
        public ActionResult SendFile(UploadFileModel fileModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            HttpPostedFileBase audioFile = fileModel.File;
            string result = string.Empty;
            string fileName = string.Empty;

            if (audioFile != null && audioFile.ContentLength > 0)
            {
                fileName = Path.GetFileName(audioFile.FileName);
                result = SendFile(fileName, audioFile.InputStream);
            }

            return View("Index", (object)string.Format("File: {0}\nResult: {1}\n", fileName, result));

        }
        private string SendFile(string path, Stream fileStream)
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

                HttpResponseMessage response = client.PostAsync("api/trackdata", content).Result;
                return response.Content.ReadAsStringAsync().Result;
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