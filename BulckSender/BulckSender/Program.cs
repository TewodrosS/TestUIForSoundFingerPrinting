using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BulckSender
{
    class Program
    {
        private static readonly List<string> filters = ConfigurationManager.AppSettings["Filters"].Split(';').ToList();

        static void Main(string[] args)
        {
            List<string> results = new List<string>();
            string outputPath = ConfigurationManager.AppSettings["OutputPath"];

            try
            {
                string path = ConfigurationManager.AppSettings["FolderLocation"];
                FileAttributes attr = File.GetAttributes(path);                

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var fileList = GetFiles(filters, path);
                    foreach (var file in fileList)
                    {
                        using (var stream = new FileStream(file, FileMode.Open))
                        {
                            var response = SendFile(file, stream);

                            results.Add(string.Format("File: {0}\nResult: {1}\n", file, response));
                        }
                    }
                }
            }
            finally
            {
                File.WriteAllLines(outputPath, results, Encoding.UTF8);
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

        public static List<string> GetPathToAllFiles(IEnumerable<string> filters, string rootFolder, bool includeSubdirectories)
        {
            if (String.IsNullOrEmpty(rootFolder))
                return null;
            List<string> fileList = new List<string>();
            try
            {
                fileList.AddRange(filters.SelectMany(filter => Directory.GetFiles(rootFolder, filter, (includeSubdirectories) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)));
            }
            catch
            {
                return null;
            }
            return fileList;
        }

        public static List<string> GetFiles(IEnumerable<string> filters, string rootFolder)
        {
            List<string> fileList = new List<string>();
            if (Directory.Exists(rootFolder))
            {
                /*If such path exists*/
                fileList = GetPathToAllFiles(filters, rootFolder, true);
            }

            return fileList;
        }

        private static string SendFile(string path, Stream fileStream)
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

    }
}
