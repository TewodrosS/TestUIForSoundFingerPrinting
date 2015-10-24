using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

    protected void Button1_Click(object sender, EventArgs e)
    {
        AudioRequest request = new AudioRequest();
        string path = uploadAudioFile.FileName;

        using (Stream fileStream = uploadAudioFile.FileContent)
        {
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
                        resultLabel.Text = "Audio Not found";
                        resultLabel.ForeColor = Color.Red;
                        resultLabel.Visible = true;

                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<TrackData>(stringResult);
                        resultLabel.Text = FormatJson(stringResult);
                        resultLabel.ForeColor = Color.Green;
                        resultLabel.Visible = true;
                    }
                }

                else
                {
                    resultLabel.Text = response.StatusCode.ToString();
                    resultLabel.ForeColor = Color.Red;
                    resultLabel.Visible = true;
                }

            }
        }
    }
}