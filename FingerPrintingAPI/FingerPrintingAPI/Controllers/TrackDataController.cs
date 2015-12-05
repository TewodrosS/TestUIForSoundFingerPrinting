using FingerPrintingAPI.Models;
using FingerPrintingAPI.Utils;
using Newtonsoft.Json;
using SoundFingerprinting;
using SoundFingerprinting.Audio;
using SoundFingerprinting.Audio.Bass;
using SoundFingerprinting.Audio.NAudio;
using SoundFingerprinting.Builder;
using SoundFingerprinting.Configuration;
using SoundFingerprinting.DAO;
using SoundFingerprinting.DAO.Data;
using SoundFingerprinting.SQL;
using SoundFingerprinting.Strides;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private const int MinTrackLength = 20; /*20 sec - minimal track length*/
        private const int MaxTrackLength = 60 * 10; /*15 min - maximal track length*/
        private IFingerprintCommandBuilder fingerprintCommandBuilder = new FingerprintCommandBuilder();
        private DefaultFingerprintConfiguration configuration = new DefaultFingerprintConfiguration();

        [HttpPost]
        public TrackDataExternal GetTrackData([FromBody]AudioRequest request)
        {
            TrackData result = null;
            string filePath = string.Empty; 
            
                try
                {
                    filePath = string.Format(ConfigurationManager.AppSettings["TempFolderTemplate"], request.FileName, request.FileType);
                    File.WriteAllBytes(filePath, request.Content);
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
            
            var stride = WinUtils.GetStride(
                    StrideType.IncrementalRandom, 
                    512, 
                    256, 
                    configuration.SamplesPerFingerprint);

            QueryResults winQueryResults = new QueryResults(
                10,
                20,
                25,
                4, 
                5,
                stride,
                tagService,
                modelService,
                audioService,
                queryCommandBuilder);

            result = winQueryResults.ExtractCandidatesWithMinHashAlgorithm(filePath);

            if (result == null)
            {
                result = new AddAudio().InsertToDatabase(filePath);
                return new TrackDataExternal(result)
                {
                    Status = "Not Found and Inserted"
                };
            }

            else
            {
                return new TrackDataExternal(result)
                {
                    Status = "Found"
                };

            }            
        }    

    }
}
