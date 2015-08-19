using FingerPrintingAPI.Models;
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
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public IHttpActionResult GetTrackData(HttpRequestMessage request)
        {
            TrackData result = null;
            string file = request.Content.ReadAsStringAsync().Result;

            DefaultFingerprintConfiguration configuration = new DefaultFingerprintConfiguration();

            QueryResults winQueryResults = new QueryResults(10, 20, 25, 4, 5,
                WinUtils.GetStride(StrideType.IncrementalRandom, 512, 256, configuration.SamplesPerFingerprint),
                tagService,
                modelService,
                audioService,
                queryCommandBuilder);

            result = winQueryResults.ExtractCandidatesWithMinHashAlgorithm(file);

            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
    }
}
