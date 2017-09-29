// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.MergeProfile
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Downloader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FreeYouTubeDownloader.Converter
{
    public class MergeProfile : ConversionProfile
    {
        public override string FormatName
        {
            get
            {
                return Path.GetExtension(this.OutputFileName).TrimStart('.').ToUpperInvariant();
            }
            set
            {
            }
        }

        public FileMergeTaskParameters MergeParameters { get; set; }

        public ConversionProfile OriginalProfile { get; set; }

        public override string OutputFileName
        {
            get
            {
                return this.MergeParameters.Output;
            }
        }

        public override IEnumerable<AudioStreamType> PreferredAudioStreamTypes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string GetFfmpegCommandArgs(VideoQualityInfo inputVideoQualityInfo)
        {
            return this.MergeParameters.GetCommandLine();
        }

        public override void ReadJson(JsonTextReader jsonTextReader)
        {
            this.MergeParameters = new FileMergeTaskParameters();
            List<string> stringList = new List<string>(2);
            while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
            {
                if (jsonTextReader.TokenType == JsonToken.PropertyName)
                {
                    string str = (string)jsonTextReader.Value;
                    if (!(str == "OutputFileName"))
                    {
                        if (!(str == "DeleteInputFile"))
                        {
                            if (str == "Inputs")
                            {
                                while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndArray)
                                {
                                    if (jsonTextReader.TokenType == JsonToken.String)
                                        stringList.Add((string)jsonTextReader.Value);
                                }
                                this.MergeParameters.Input = stringList.ToArray();
                            }
                        }
                        else
                        {
                            jsonTextReader.Read();
                            this.DeleteInputFile = (bool)jsonTextReader.Value;
                        }
                    }
                    else
                        this.MergeParameters.Output = jsonTextReader.ReadAsString();
                }
            }
        }

        public override void WriteJson(JsonTextWriter jsonTextWriter)
        {
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WritePropertyName("$type");
            jsonTextWriter.WriteValue(this.GetType().ToString());
            jsonTextWriter.WritePropertyName("OutputFileName");
            jsonTextWriter.WriteValue(this.OutputFileName);
            jsonTextWriter.WritePropertyName("DeleteInputFile");
            jsonTextWriter.WriteValue(this.DeleteInputFile);
            jsonTextWriter.WritePropertyName("Inputs");
            jsonTextWriter.WriteStartArray();
            foreach (string str in this.MergeParameters.Input)
                jsonTextWriter.WriteValue(str);
            jsonTextWriter.WriteEndArray();
            jsonTextWriter.WriteEndObject();
        }
    }
}
