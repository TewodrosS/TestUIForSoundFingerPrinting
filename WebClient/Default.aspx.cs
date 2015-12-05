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
    private readonly List<string> filters = new List<string>(new[] { "*.mp3", "*.wav", "*.ogg", "*.flac" }); /*File filters*/
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

    protected void SendFile_Click(object sender, EventArgs e)
    {
        string path = uploadAudioFile.FileName;
        using (var stream = uploadAudioFile.FileContent)
        {
            SendFile(path, stream);
        }

        //FileAttributes attr = File.GetAttributes(path);

        //if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
        //{
        //    var fileList = GetFiles(filters, path);
        //    foreach (var file in fileList)
        //    {
        //        SendFile(file);
        //    }
        //}        
    }

    protected void SendMultiple_Click(object sender, EventArgs e)
    {
        string path = directoryInput.Value;
        FileAttributes attr = File.GetAttributes(path);

        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
        {
            var fileList = GetFiles(filters, path);
            foreach (var file in fileList)
            {
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    SendFile(file, stream);
                }
            }
        }


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
                    resultLabel.Text = resultLabel.Text + "<br /> <br /> <br />" + "Audio Not found";
                    resultLabel.ForeColor = Color.Red;
                    resultLabel.Visible = true;

                }
                else
                {
                    var result = JsonConvert.DeserializeObject<TrackData>(stringResult);
                    resultLabel.Text = resultLabel.Text + "<br /> <br /> <br />" + FormatJson(stringResult).Replace(",", "<br />"); ;
                    resultLabel.ForeColor = Color.Green;
                    resultLabel.Visible = true;
                }
            }

            else
            {
                resultLabel.Text = resultLabel.Text + "<br /> <br /> <br />" + response.StatusCode.ToString();
                resultLabel.ForeColor = Color.Red;
                resultLabel.Visible = true;
            }
        }
    }
}