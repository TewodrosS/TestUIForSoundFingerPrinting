using FingerPrintingAPI.Models;
using Newtonsoft.Json;
using SoundFingerprinting;
using SoundFingerprinting.Audio;
using SoundFingerprinting.Audio.Bass;
using SoundFingerprinting.Audio.NAudio;
using SoundFingerprinting.Builder;
using SoundFingerprinting.Configuration;
using SoundFingerprinting.DAO.Data;
using SoundFingerprinting.SQL;
using SoundFingerprinting.Strides;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Http;

namespace FingerPrintingAPI.Controllers
{
    public class TrackDataController : ApiController
    {
        private IQueryCommandBuilder queryCommandBuilder = new QueryCommandBuilder();
        private ITagService tagService = new BassTagService();
        private IModelService modelService = new SqlModelService();
        private IAudioService audioService = new NAudioService();
        IFingerprintCommandBuilder fingerprintCommandBuilder = new FingerprintCommandBuilder();

        [HttpPost]
        public TrackDataExternal GetTrackData(string file)
        {
            TrackData result = null;
            string filePath = string.Empty;

            var task = this.Request.Content.ReadAsStreamAsync();
            task.Wait();

            int length = (int)task.Result.Length;
            using (Stream requestStream = task.Result)
            {
                try
                {
                    filePath = @"D:\Dev\Temp\" + file + DateTime.Now.Ticks;
                    using (Stream fileStream = File.Create(filePath, length))
                    {
                        requestStream.CopyTo(fileStream);
                        fileStream.Close();
                        requestStream.Close();
                    }
                }
                catch (IOException)
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage
                        {
                            ReasonPhrase = "Could not read the request",
                            StatusCode = HttpStatusCode.InternalServerError
                        });
                }
            }

            DefaultFingerprintConfiguration configuration = new DefaultFingerprintConfiguration();

            QueryResults winQueryResults = new QueryResults(10, 20, 25, 4, 5,
                WinUtils.GetStride(StrideType.IncrementalRandom, 512, 256, configuration.SamplesPerFingerprint),
                tagService,
                modelService,
                audioService,
                queryCommandBuilder);

            result = winQueryResults.ExtractCandidatesWithMinHashAlgorithm(filePath);

            if (result == null)
                return null;
                
            

            return new TrackDataExternal(result);

            //if (result == null)
            //    return NotFound();
            //else
            //    return Ok(result);
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
