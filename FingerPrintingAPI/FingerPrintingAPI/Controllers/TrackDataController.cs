using FingerPrintingAPI.Models;
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

            QueryResults winQueryResults = new QueryResults(10, 20, 25, 4, 5,
                WinUtils.GetStride(StrideType.IncrementalRandom, 512, 256, configuration.SamplesPerFingerprint),
                tagService,
                modelService,
                audioService,
                queryCommandBuilder);

            result = winQueryResults.ExtractCandidatesWithMinHashAlgorithm(filePath);

            if (result == null)
            {
                result = InsertToDatabase(filePath);
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

        private TrackData InsertToDatabase(string filePath)
        {
            TrackData trackData = null;

            IStride stride = WinUtils.GetStride(StrideType.IncrementalRandom, 5115, 0, configuration.SamplesPerFingerprint);

            TagInfo tags = tagService.GetTagInfo(filePath); // Get Tags from file
            if (tags == null || tags.IsEmpty)
            {
                // tags null
            }

            string isrc = tags.ISRC;
            string artist = tags.Artist; // Artist
            string title = tags.Title; // Title
            int releaseYear = tags.Year;
            string album = tags.Album;
            double duration = tags.Duration; // Duration

            // Check whether the duration is OK
            if (duration < MinTrackLength || duration > MaxTrackLength)
            {
                // Duration too small                    
            }

            // Check whether the tags are properly defined
            if (string.IsNullOrEmpty(isrc) && (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title)))
            {
                //"ISRC Tag is missing. Skipping file..."
            }

            IModelReference trackReference;
            try
            {
                lock (this)
                {
                    // Check if this file is already in the database
                    if (IsDuplicateFile(isrc, artist, title))
                    {
                        //duplicate file exist
                    }

                    trackData = new TrackData(isrc, artist, title, album, releaseYear, (int)duration);
                    trackReference = modelService.InsertTrack(trackData); // Insert new Track in the database                    
                }
            }
            catch (Exception e)
            {
                // catch any exception and abort the insertion
                return trackData;
            }

            int count;
            try
            {
                var hashDatas = fingerprintCommandBuilder
                                    .BuildFingerprintCommand()
                                    .From(filePath)
                                    .WithFingerprintConfig(
                                        config =>
                                        {
                                            config.TopWavelets = 200;
                                            config.SpectrogramConfig.Stride = stride;
                                        })
                                    .UsingServices(audioService)
                                    .Hash()
                                    .Result; // Create SubFingerprints

                modelService.InsertHashDataForTrack(hashDatas, trackReference);
                count = hashDatas.Count;
                return trackData;
            }
            catch (Exception e)
            {
                // catch any exception and abort the insertion
                return null;
            }

        }

        private bool IsDuplicateFile(string isrc, string artist, string title)
        {
            if (!string.IsNullOrEmpty(isrc))
            {
                return modelService.ReadTrackByISRC(isrc) != null;
            }

            return modelService.ReadTrackByArtistAndTitleName(artist, title).Any();
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
