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
using System.Linq;
using System.Web;

namespace FingerPrintingAPI.Utils
{
    public class AddAudio
    {
        private IQueryCommandBuilder queryCommandBuilder = new QueryCommandBuilder();
        private ITagService tagService = new BassTagService();
        private IModelService modelService = new SqlModelService();
        private IAudioService audioService = new NAudioService();
        private const int MinTrackLength = 20; /*20 sec - minimal track length*/
        private const int MaxTrackLength = 60 * 10; /*15 min - maximal track length*/
        private IFingerprintCommandBuilder fingerprintCommandBuilder = new FingerprintCommandBuilder();
        private DefaultFingerprintConfiguration configuration = new DefaultFingerprintConfiguration();

        public TrackData InsertToDatabase(string filePath)
        {
            TrackData trackData = null;

            IStride stride = WinUtils.GetStride(StrideType.IncrementalRandom, 5115, 0, configuration.SamplesPerFingerprint);

            TagInfo tags = tagService.GetTagInfo(filePath); // Get Tags from file
            if (tags == null || tags.IsEmpty)
            {
                // tags null
            }

            //string isrc = tags.ISRC;
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
            
            IModelReference trackReference;
            try
            {
                lock (this)
                {                    
                    trackData = new TrackData(artist, title, album, releaseYear, (int)duration);
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

        //private bool IsDuplicateFile(string isrc, string artist, string title)
        //{
        //    if (!string.IsNullOrEmpty(isrc))
        //    {
        //        return modelService.ReadTrackByISRC(isrc) != null;
        //    }

        //    return modelService.ReadTrackByArtistAndTitleName(artist, title).Any();
        //}
    }
}