using SoundFingerprinting;
using SoundFingerprinting.Audio;
using SoundFingerprinting.Builder;
using SoundFingerprinting.DAO.Data;
using SoundFingerprinting.Strides;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FingerPrintingAPI.Utils
{
    public class QueryResults
    {
        private const int MinTrackLength = 5;
        private const int MaxTrackLength = 60 * 20;

        private const string ColSongName = "SongNameTitle";
        private const string ColResultName = "ResultSongNameTitle";
        private const string ColResult = "Result";
        private const string ColHammingAvg = "HammingAvg";
        private const string ColNumberOfCandidates = "TotalNumberOfAnalyzedCandidates";
        private const string ColISRC = "ISRC";

        private readonly int hashKeys;
        private readonly int hashTables;
        private readonly int secondsToAnalyze;
        private readonly int startSecond;
        private readonly int threshold;

        private readonly IStride queryStride;
        private readonly ITagService tagService;
        private readonly IModelService modelService;
        private readonly IAudioService audioService;
        private readonly IQueryCommandBuilder queryCommandBuilder;

        public QueryResults(
            int secondsToAnalyze,
            int startSecond,
            int hashTables,
            int hashKeys,
            int threshold,
            IStride queryStride,
            ITagService tagService,
            IModelService modelService,
            IAudioService audioService,
            IQueryCommandBuilder queryCommandBuilder)
        {
            this.secondsToAnalyze = secondsToAnalyze;
            this.startSecond = startSecond;
            this.queryStride = queryStride;
            this.hashTables = hashTables;
            this.hashKeys = hashKeys;
            this.threshold = threshold;
            this.tagService = tagService;
            this.modelService = modelService;
            this.audioService = audioService;
            this.queryCommandBuilder = queryCommandBuilder;

        }

        public TrackData ExtractCandidatesWithMinHashAlgorithm(string pathToFile)
        {
            int verified = 0;
            
            TagInfo tags = tagService.GetTagInfo(pathToFile); // Get Tags from file

            if (tags == null || tags.IsEmpty)
            {
                return null;
            }

            string artist = string.IsNullOrEmpty(tags.Artist)
                                ? Path.GetFileNameWithoutExtension(pathToFile)
                                : tags.Artist; // Artist
            string title = string.IsNullOrEmpty(tags.Title)
                               ? Path.GetFileNameWithoutExtension(pathToFile)
                               : tags.Title; // Title
            string isrc = tags.ISRC;
            double duration = tags.Duration; // Duration

            // Check whether the duration is ok
            if (duration < MinTrackLength || duration > MaxTrackLength || secondsToAnalyze > duration)
            {
                return null;
            }

            var queryResult =
                queryCommandBuilder.BuildQueryCommand()
                                       .From(pathToFile, secondsToAnalyze, startSecond)
                                       .WithConfigs(
                                            fingerprintConfig =>
                                            {
                                                fingerprintConfig.HashingConfig.NumberOfLSHTables = hashTables;
                                                fingerprintConfig.HashingConfig.NumberOfMinHashesPerTable = hashKeys;
                                                fingerprintConfig.SpectrogramConfig.Stride = queryStride;
;
                                            },
                                            queryConfig =>
                                            {
                                                queryConfig.ThresholdVotes = threshold;
                                            })
                                        .UsingServices(modelService, audioService)
                                        .Query()
                                        .Result;
            
            if (!queryResult.IsSuccessful)
            {
                File.Delete(pathToFile);
                return null;
            }

            verified++;
            TrackData recognizedTrack = queryResult.BestMatch.Track;

            File.Delete(pathToFile);
            return recognizedTrack;

        }

    }
}