// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Tasks.MultifileDownloadTask
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeYouTubeDownloader.Downloader.Tasks
{
    public sealed class MultifileDownloadTask : DownloadTask
    {
        private readonly object _locker = new object();
        private List<SinglefileDownloadTask> _singlefileDownloads;
        private DownloadState _lastDownloadState;
        private bool _errorThrown;
        public List<MultifileDownloadParameter> MultifileDownloadParameters;

        public override long BytesDownloaded
        {
            get
            {
                List<SinglefileDownloadTask> singlefileDownloads = this._singlefileDownloads;
                if (singlefileDownloads == null)
                    return 0;
                return singlefileDownloads.Sum<SinglefileDownloadTask>((Func<SinglefileDownloadTask, long>)(download => download.BytesDownloaded));
            }
        }

        public override long BytesTotal
        {
            get
            {
                List<SinglefileDownloadTask> singlefileDownloads = this._singlefileDownloads;
                if (singlefileDownloads == null)
                    return 0;
                return singlefileDownloads.Sum<SinglefileDownloadTask>((Func<SinglefileDownloadTask, long>)(download => download.BytesTotal));
            }
            protected set
            {
            }
        }

        public List<SinglefileDownloadTask> SinglefileDownloads
        {
            get
            {
                return this._singlefileDownloads;
            }
        }

        public override void Download(MediaLink link, string downloadedMediaFileLocation, long range = -1)
        {
            throw new NotImplementedException();
        }

        public override void MultifileDownload(string downloadedMediaFileLocation, List<MultifileDownloadParameter> multifileDownloadParameters)
        {
            this.FileName = downloadedMediaFileLocation;
            this.MultifileDownloadParameters = multifileDownloadParameters;
            this._singlefileDownloads = new List<SinglefileDownloadTask>(multifileDownloadParameters.Count);
            foreach (MultifileDownloadParameter downloadParameter in multifileDownloadParameters)
            {
                SinglefileDownloadTask singlefileDownloadTask = new SinglefileDownloadTask();
                singlefileDownloadTask.DownloadError += new DownloadTask.DownloadErrorEventHandler(this.OnSinglefileDownloadError);
                singlefileDownloadTask.DownloadFinished += new DownloadTask.DownloadFinishedEventHandler(this.OnSinglefileDownloadFinished);
                singlefileDownloadTask.DownloadProgress += new DownloadTask.DownloadProgressEventHandler(this.OnSinglefileDownloadProgress);
                singlefileDownloadTask.DownloadStateChanged += new DownloadTask.DownloadStateChangedEventHandler(this.OnSinglefileDownloadStateChanged);
                this._singlefileDownloads.Add(singlefileDownloadTask);
                singlefileDownloadTask.Download(downloadParameter.Link, downloadParameter.FileName, (long)downloadParameter.ByteRange);
            }
        }

        public override void Abort()
        {
            if (this._singlefileDownloads == null)
                return;
            foreach (DownloadTask singlefileDownload in this._singlefileDownloads)
                singlefileDownload.Abort();
        }

        public override void Pause()
        {
            foreach (DownloadTask downloadTask in this._singlefileDownloads.Where<SinglefileDownloadTask>((Func<SinglefileDownloadTask, bool>)(download => !download.IsCompleted)))
                downloadTask.Pause();
        }

        public override void Resume()
        {
            this._errorThrown = false;
            List<SinglefileDownloadTask> singlefileDownloads = this._singlefileDownloads;
            Func<SinglefileDownloadTask, bool> func = (Func<SinglefileDownloadTask, bool>)(download => download.IsCompleted);
            Func<SinglefileDownloadTask, bool> predicate = null;
            if (singlefileDownloads.All<SinglefileDownloadTask>(predicate))
                this.NotifyDownloadFinished(new DownloadFinishedEventArgs(DownloadState.Completed));
            foreach (DownloadTask downloadTask in this._singlefileDownloads.Where<SinglefileDownloadTask>((Func<SinglefileDownloadTask, bool>)(download => !download.IsCompleted)))
                downloadTask.Resume();
            this.PausedByNetworkState = false;
        }

        private void OnSinglefileDownloadStateChanged(object sender, DownloadState state)
        {
            lock (this._locker)
            {
                if (this._lastDownloadState == state)
                    return;
                this.NotifyDownloadStateChanged(state);
                this._lastDownloadState = state;
            }
        }

        private void OnSinglefileDownloadProgress(object sender, DownloadProgressEventArgs downloadProgressEventArgs)
        {
            List<SinglefileDownloadTask> list = this._singlefileDownloads.Where<SinglefileDownloadTask>((Func<SinglefileDownloadTask, bool>)(downloader => !downloader.IsCompleted)).ToList<SinglefileDownloadTask>();
            int num = list.Count > 0 ? list.Count : 1;
            this.NotifyDownloadProgress(new DownloadProgressEventArgs((int)((double)this.BytesDownloaded / (double)this.BytesTotal * 100.0), list.Sum<SinglefileDownloadTask>((Func<SinglefileDownloadTask, long>)(download => download.DownloadSpeed)) / (long)num, list.Sum<SinglefileDownloadTask>((Func<SinglefileDownloadTask, long>)(download => download.SecondsRemains)) / (long)num));
        }

        private void OnSinglefileDownloadFinished(object sender, DownloadFinishedEventArgs downloadFinishedEventArgs)
        {
            lock (this._locker)
            {
                if (this.IsUsed)
                    return;
                List<SinglefileDownloadTask> singlefileDownloads = this._singlefileDownloads;
                Func<SinglefileDownloadTask, bool> func = (Func<SinglefileDownloadTask, bool>)(download => download.IsCompleted);
                Func<SinglefileDownloadTask, bool> predicate = null;
                if (!singlefileDownloads.All<SinglefileDownloadTask>(predicate))
                    return;
                this.IsUsed = true;
                this.NotifyDownloadFinished(downloadFinishedEventArgs);
            }
        }

        private void OnSinglefileDownloadError(object sender, DownloadErrorEventArgs downloadErrorEventArgs)
        {
            SinglefileDownloadTask download = (SinglefileDownloadTask)sender;
            foreach (DownloadTask downloadTask in this._singlefileDownloads.Where<SinglefileDownloadTask>((Func<SinglefileDownloadTask, bool>)(d => d != download)))
                downloadTask.Abort();
            if (this._errorThrown)
                return;
            this.NotifyDownloadError(downloadErrorEventArgs);
            this._errorThrown = true;
        }

        public override void ReadJson(JsonTextReader jsonTextReader)
        {
            label_17:
            while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
            {
                if (jsonTextReader.TokenType == JsonToken.PropertyName)
                {
                    string str = (string)jsonTextReader.Value;
                    if (!(str == "Id"))
                    {
                        if (!(str == "FileName"))
                        {
                            if (!(str == "Downloads"))
                            {
                                if (str == "MultifileDownloadParameters")
                                {
                                    this.MultifileDownloadParameters = new List<MultifileDownloadParameter>();
                                    while (true)
                                    {
                                        do
                                        {
                                            if (!jsonTextReader.Read() || jsonTextReader.TokenType == JsonToken.EndArray)
                                                goto label_17;
                                        }
                                        while (jsonTextReader.TokenType != JsonToken.PropertyName || !((string)jsonTextReader.Value == "$type"));
                                        MultifileDownloadParameter instance = (MultifileDownloadParameter)Activator.CreateInstance(Type.GetType(jsonTextReader.ReadAsString()));
                                        instance.ReadJson(jsonTextReader);
                                        this.MultifileDownloadParameters.Add(instance);
                                    }
                                }
                            }
                            else
                            {
                                this._singlefileDownloads = new List<SinglefileDownloadTask>();
                                while (true)
                                {
                                    do
                                    {
                                        if (!jsonTextReader.Read() || jsonTextReader.TokenType == JsonToken.EndArray)
                                            goto label_17;
                                    }
                                    while (jsonTextReader.TokenType != JsonToken.PropertyName || !((string)jsonTextReader.Value == "$type"));
                                    SinglefileDownloadTask instance = (SinglefileDownloadTask)Activator.CreateInstance(Type.GetType(jsonTextReader.ReadAsString()));
                                    instance.ReadJson(jsonTextReader);
                                    instance.DownloadError += new DownloadTask.DownloadErrorEventHandler(this.OnSinglefileDownloadError);
                                    instance.DownloadFinished += new DownloadTask.DownloadFinishedEventHandler(this.OnSinglefileDownloadFinished);
                                    instance.DownloadProgress += new DownloadTask.DownloadProgressEventHandler(this.OnSinglefileDownloadProgress);
                                    instance.DownloadStateChanged += new DownloadTask.DownloadStateChangedEventHandler(this.OnSinglefileDownloadStateChanged);
                                    this._singlefileDownloads.Add(instance);
                                }
                            }
                        }
                        else
                            this.FileName = jsonTextReader.ReadAsString();
                    }
                    else
                        this.Id = jsonTextReader.ReadAsString();
                }
            }
        }

        public override void WriteJson(JsonTextWriter jsonTextWriter)
        {
            this.Abort();
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WritePropertyName("$type");
            jsonTextWriter.WriteValue(this.GetType().ToString());
            jsonTextWriter.WritePropertyName("Id");
            jsonTextWriter.WriteValue(this.Id);
            jsonTextWriter.WritePropertyName("FileName");
            jsonTextWriter.WriteValue(this.FileName);
            if (this._singlefileDownloads != null)
            {
                jsonTextWriter.WritePropertyName("Downloads");
                jsonTextWriter.WriteStartArray();
                foreach (DownloadTask singlefileDownload in this._singlefileDownloads)
                    singlefileDownload.WriteJson(jsonTextWriter);
                jsonTextWriter.WriteEndArray();
            }
            if (this.MultifileDownloadParameters != null)
            {
                jsonTextWriter.WritePropertyName("MultifileDownloadParameters");
                jsonTextWriter.WriteStartArray();
                foreach (MultifileDownloadParameter downloadParameter in this.MultifileDownloadParameters)
                    downloadParameter.WriteJson(jsonTextWriter);
                jsonTextWriter.WriteEndArray();
            }
            jsonTextWriter.WriteEndObject();
        }

        public override void UpdateAfterDeserialization()
        {
            foreach (DownloadTask downloadTask in this._singlefileDownloads.Where<SinglefileDownloadTask>((Func<SinglefileDownloadTask, bool>)(download => !download.IsCompleted)))
                downloadTask.UpdateAfterDeserialization();
        }
    }
}
