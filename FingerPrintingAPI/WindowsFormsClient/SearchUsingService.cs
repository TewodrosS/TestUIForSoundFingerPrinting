using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsClient
{
    public partial class SearchUsingService : Form
    {

        private readonly List<string> filters = new List<string>(new[] { "*.mp3", "*.wav", "*.ogg", "*.flac" });
        
        public SearchUsingService()
        {            
            InitializeComponent();
            textBoxUrl.Text = "http://localhost:7375/";
            resultLable.Visible = false;
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            string filter = GetMultipleFilter("Audio files", filters);
            OpenFileDialog ofd = new OpenFileDialog { Filter = filter, Multiselect = false };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxFilePath.Text = null;

                textBoxFilePath.Text = ofd.FileName;
                
                _btnSend.Enabled = true;
                    
            }
        }


        public static string GetMultipleFilter(string caption, IEnumerable<string> filters)
        {
            StringBuilder filter = new StringBuilder(caption);
            filter.Append(" (");
            for (int i = 0; i < filters.Count(); i++)
            {
                filter.Append(filters.ElementAt(i));
                if (i != filters.Count() - 1 /*last*/)
                    filter.Append(";");
                else
                {
                    filter.Append(")|");
                    for (int j = 0; j < filters.Count(); j++)
                    {
                        filter.Append(filters.ElementAt(j));
                        if (j != filters.Count() - 1 /*last*/)
                            filter.Append(";");
                    }
                }
            }
            return filter.ToString();
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

        private static string FormatJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        private void Send_Click(object sender, EventArgs e)
        {
            resultLable.Visible = false;

            string url = textBoxUrl.Text;
            string path = textBoxFilePath.Text;

            using(MemoryStream requestStream = new MemoryStream())
            using (Stream fileStream = File.Open(path, FileMode.Open))
            {
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

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string stringResult = response.Content.ReadAsStringAsync().Result;

                        if (string.IsNullOrWhiteSpace(stringResult) || stringResult == "null")
                        {
                            resultLable.Text = "Audio Not found";
                            resultLable.ForeColor = Color.Yellow;
                            resultLable.Visible = true;

                        }
                        else
                        {
                            var result = JsonConvert.DeserializeObject<TrackData>(stringResult);
                            resultLable.Text = FormatJson(stringResult);
                            resultLable.ForeColor = Color.Green;
                            resultLable.Visible = true;
                        }
                    }

                    else
                    {
                        resultLable.Text = response.StatusCode.ToString();
                        resultLable.ForeColor = Color.Red;
                        resultLable.Visible = true;
                    }

                }
            }
        }
    }
        
}

