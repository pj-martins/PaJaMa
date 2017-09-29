// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Tasks.SinglefileDownloadTask
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Debug;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace FreeYouTubeDownloader.Downloader.Tasks
{
    public sealed class SinglefileDownloadTask : DownloadTask
    {
        private long _bytesDownloaded = -1;
        private ManualResetEvent _waitObject;
        private volatile bool _pendingCancellation;

        public override long BytesDownloaded
        {
            get
            {
                return this._bytesDownloaded;
            }
        }

        public override void Download(MediaLink link, string downloadedMediaFileLocation, long range = -1)
        {
            Log.Trace("CALL SinglefileDownloadTask.Download(string, string, long)", (Exception)null);
            if (link == null && this.SourceUrl == null)
            {
                this.NotifyDownloadError(new DownloadErrorEventArgs(FreeYouTubeDownloader.Downloader.DownloadError.Unknown, (Exception)null));
            }
            else
            {
                if (link != null)
                {
                    link.UpdateLink();
                    this.Link = link;
                }
                this.FileName = downloadedMediaFileLocation;
                ThreadPool.QueueUserWorkItem((WaitCallback)(mockState =>
               {
                   try
                   {
                       this._pendingCancellation = false;
                       this._waitObject = new ManualResetEvent(false);
                       HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(link != null ? link.Url : this.SourceUrl, UriKind.Absolute));
                       if (range > 0L)
                           httpWebRequest.AddRange(range);
                       else
                           this._bytesDownloaded = 0L;
                       DownloadTaskResponseDataObject responseDataObject1 = new DownloadTaskResponseDataObject();
                       responseDataObject1.Request = httpWebRequest;
                       long num = range;
                       responseDataObject1.Range = num;
                       DownloadTaskResponseDataObject responseDataObject2 = responseDataObject1;
                       httpWebRequest.ReadWriteTimeout = 30000;
                       httpWebRequest.BeginGetResponse(new AsyncCallback(this.GetDataCallback), (object)responseDataObject2);
                   }
                   catch (Exception ex)
                   {
                       Log.Error("SinglefileDownloadTask.Download => " + ex.Message, (Exception)null);
                       this.NotifyDownloadError(new DownloadErrorEventArgs(FreeYouTubeDownloader.Downloader.DownloadError.Unknown, ex));
                   }
                   if (!string.IsNullOrEmpty(this.Id))
                       return;
                   this.Id = Guid.NewGuid().ToString();
               }), (object)null);
            }
        }

        public override void MultifileDownload(string downloadedMediaFileLocation, List<MultifileDownloadParameter> multifileDownloadParameters)
        {
            throw new NotImplementedException();
        }

        public override void Abort()
        {
            Log.Trace("CALL SinglefileDownloadTask.Abort()", (Exception)null);
            if (this._pendingCancellation || this._waitObject == null)
                return;
            this._pendingCancellation = true;
            this._waitObject.WaitOne();
        }

        public override void Pause()
        {
            Log.Trace("CALL SinglefileDownloadTask.Pause()", (Exception)null);
            this.Abort();
            this.NotifyDownloadStateChanged(DownloadState.Paused);
        }

        public override void Resume()
        {
            Log.Trace("CALL SinglefileDownloadTask.Resume()", (Exception)null);
            this.Download(this.Link, this.FileName, this._bytesDownloaded);
            this.PausedByNetworkState = false;
        }

        public override void ReadJson(JsonTextReader jsonTextReader)
        {
            while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
            {
                if (jsonTextReader.TokenType == JsonToken.PropertyName)
                {
                    string s = (string)jsonTextReader.Value;
                    // ISSUE: reference to a compiler-generated method
                    //uint stringHash = "\u003CPrivateImplementationDetails\u003E".ComputeStringHash(s);
                    //if (stringHash <= 1610471560U)
                    //{
                    //    if ((int)stringHash != 439787878)
                    //    {
                    //        if ((int)stringHash != 921221376)
                    //        {
                    //            if ((int)stringHash == 1610471560 && s == "FileName")
                    if (s == "FileName")
                        this.FileName = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "Id")
                        this.Id = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "Progress")
                        this.Progress = jsonTextReader.ReadAsInt32().GetValueOrDefault(0);
                    //}
                    //else if (stringHash <= 2501501886U)
                    //{
                    //    if ((int)stringHash != -1912019575)
                    //    {
                    //        if ((int)stringHash == -1793465410 && s == "Url")
                    else if (s == "Url")
                        this.SourceUrl = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "Link")
                    {
                        jsonTextReader.Read();
                        jsonTextReader.Read();
                        this.Link = (MediaLink)Activator.CreateInstance(Assembly.Load("FreeYouTubeDownloader.Downloader").GetType(jsonTextReader.ReadAsString()));
                        this.Link.ReadJson(jsonTextReader);
                    }
                    //}
                    //else if ((int)stringHash != -1358204163)
                    //{
                    //    if ((int)stringHash == -646558010 && s == "BytesTotal")
                    else if (s == "BytesTotal")
                    {
                        jsonTextReader.Read();
                        this.BytesTotal = (long)jsonTextReader.Value;
                    }
                    //}
                    else if (s == "BytesDownloaded")
                    {
                        jsonTextReader.Read();
                        this._bytesDownloaded = (long)jsonTextReader.Value;
                    }
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
            jsonTextWriter.WritePropertyName("BytesDownloaded");
            jsonTextWriter.WriteValue(this._bytesDownloaded);
            jsonTextWriter.WritePropertyName("Url");
            jsonTextWriter.WriteValue(this.SourceUrl);
            if (this.Link != null)
            {
                jsonTextWriter.WritePropertyName("Link");
                this.Link.WriteJson(jsonTextWriter);
            }
            jsonTextWriter.WritePropertyName("FileName");
            jsonTextWriter.WriteValue(this.FileName);
            jsonTextWriter.WritePropertyName("BytesTotal");
            jsonTextWriter.WriteValue(this.BytesTotal);
            jsonTextWriter.WritePropertyName("Progress");
            jsonTextWriter.WriteValue(this.Progress);
            jsonTextWriter.WriteEndObject();
        }

        private void GetDataCallback(IAsyncResult result)
        {
            Log.Trace("CALL SinglefileDownloadTask.GetDataCallback(IAsyncResult)", (Exception)null);
            DownloadTaskResponseDataObject asyncState = (DownloadTaskResponseDataObject)result.AsyncState;
            HttpWebRequest request = asyncState.Request;
            long range = asyncState.Range;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                if (this._pendingCancellation)
                {
                    this.Cleanup((IDisposable)response);
                    this._waitObject.Set();
                }
                else
                {
                    Stream responseStream = response.GetResponseStream();
                    DateTime downloadStarted = DateTime.UtcNow;
                    byte[] buffer = new byte[524288];
                    long bytesDownloadedSoFar = 0;
                    long totalFileSize = this._bytesDownloaded > 0L ? response.ContentLength + this._bytesDownloaded : response.ContentLength;
                    this.BytesTotal = totalFileSize;
                    if (FileSystemUtil.GetTotalFreeSpace(this.FileName) < totalFileSize)
                    {
                        this.Cleanup((IDisposable)responseStream, (IDisposable)response);
                        this.NotifyDownloadError(new DownloadErrorEventArgs(FreeYouTubeDownloader.Downloader.DownloadError.NotEnoughFreeSpace, (Exception)null));
                        this._waitObject.Set();
                    }
                    else if (this._pendingCancellation)
                    {
                        this.Cleanup((IDisposable)responseStream, (IDisposable)response);
                        this._waitObject.Set();
                    }
                    else
                    {
                        string directoryName = Path.GetDirectoryName(this.FileName);
                        if (!Directory.Exists(directoryName))
                            Directory.CreateDirectory(directoryName);
                        FileStream fileStream;
                        if (range > 0L)
                        {
                            fileStream = new FileStream(this.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                            fileStream.Seek(range, SeekOrigin.Begin);
                        }
                        else
                            fileStream = new FileStream(this.FileName, FileMode.Create, FileAccess.Write);
                        System.Threading.Timer timer = new System.Threading.Timer((TimerCallback)(timerState =>
                       {
                           try
                           {
                               if (fileStream.Length == 0L)
                                   return;
                               double totalSeconds = (DateTime.UtcNow - downloadStarted).TotalSeconds;
                               long int64_1 = Convert.ToInt64((double)bytesDownloadedSoFar / totalSeconds);
                               long int64_2 = Convert.ToInt64((double)(response.ContentLength - bytesDownloadedSoFar) / ((double)bytesDownloadedSoFar / totalSeconds));
                               this.NotifyDownloadProgress(fileStream.Length, totalFileSize, int64_1, int64_2);
                               if (int64_2 != 0L)
                                   return;
                               this._waitObject.Set();
                           }
                           catch
                           {
                           }
                       }), (object)null, 2000, 2000);
                        this.NotifyDownloadStateChanged(DownloadState.Downloading);
                        try
                        {
                            int count;
                            while ((count = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                if (!this._pendingCancellation)
                                {
                                    fileStream.Write(buffer, 0, count);
                                    this._bytesDownloaded = fileStream.Length;
                                    bytesDownloadedSoFar += (long)count;
                                }
                                else
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Cleanup((IDisposable)timer, (IDisposable)responseStream, (IDisposable)response, (IDisposable)fileStream);
                            throw;
                        }
                        if (this._pendingCancellation)
                        {
                            this.Cleanup((IDisposable)timer, (IDisposable)responseStream, (IDisposable)response, (IDisposable)fileStream);
                            this._waitObject.Set();
                        }
                        else if (fileStream.Length < totalFileSize)
                        {
                            Log.Warning("The file wasn't downloaded completely", (Exception)null);
                            this.Cleanup((IDisposable)timer, (IDisposable)responseStream, (IDisposable)response, (IDisposable)fileStream);
                            this.NotifyDownloadError(new DownloadErrorEventArgs(FreeYouTubeDownloader.Downloader.DownloadError.Unknown, (Exception)null));
                            this._waitObject.Set();
                        }
                        else
                        {
                            this._waitObject.WaitOne();
                            this.Cleanup((IDisposable)timer, (IDisposable)responseStream, (IDisposable)response, (IDisposable)fileStream);
                            this.NotifyDownloadFinished(new DownloadFinishedEventArgs(DownloadState.Completed));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("SinglefileDownloadTask.GetDataCallback => " + ex.Message, ex);
                this._pendingCancellation = true;
                this.NotifyDownloadError(new DownloadErrorEventArgs(FreeYouTubeDownloader.Downloader.DownloadError.Unknown, ex));
            }
            finally
            {
                this._waitObject.Set();
            }
        }

        public override void UpdateAfterDeserialization()
        {
            Log.Trace("CALL SinglefileDownloadTask.UpdateAfterDeserialization()", (Exception)null);
            if (this.Link == null || string.IsNullOrEmpty(this.FileName) || this._bytesDownloaded <= 0L)
                return;
            this.Download(this.Link, this.FileName, System.IO.File.Exists(this.FileName) ? this._bytesDownloaded : 0L);
        }
    }
}
