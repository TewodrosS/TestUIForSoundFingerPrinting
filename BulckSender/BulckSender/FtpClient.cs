using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BulckSender
{
    public class FtpClient
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string FtpAddress;

        public FtpClient(string userName, string password, string ftpAddress)
        {
            UserName = userName;
            Password = password;
            FtpAddress = ftpAddress;
            
        }

        public List<string> GetFiles(List<string> filters)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest requestDir = (FtpWebRequest)WebRequest.Create(FtpAddress);
            requestDir.Method = WebRequestMethods.Ftp.ListDirectory;
            requestDir.Credentials = new NetworkCredential(UserName, Password);
            FtpWebResponse responseDir = (FtpWebResponse)requestDir.GetResponse();
            StreamReader readerDir = new StreamReader(responseDir.GetResponseStream());

            string line = readerDir.ReadLine();
            while (line != null)
            {
                result.Append(line);
                result.Append("\n");
                line = readerDir.ReadLine();
            }

            result.Remove(result.ToString().LastIndexOf('\n'), 1);
            responseDir.Close();
            var allFiles =  result.ToString().Split('\n');

            return allFiles.Where(file => filters.Any(filter => file.EndsWith(filter))).ToList();
        }

        public Stream GetFileStream(string file)
        {                       
            try
            {
                string uri = FtpAddress + file;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    return null;
                }       
                FtpWebRequest reqFTP;                
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));                                
                reqFTP.Credentials = new NetworkCredential(UserName, Password);                
                reqFTP.KeepAlive = false;                
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;                                
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;                 
                reqFTP.UsePassive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream responseStream = response.GetResponseStream();

                return responseStream;
            }
            catch (WebException wEx)
            {
                //MessageBox.Show(wEx.Message, "Download Error");
                return null;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Download Error");
                return null;
            }
        }
    }
}
