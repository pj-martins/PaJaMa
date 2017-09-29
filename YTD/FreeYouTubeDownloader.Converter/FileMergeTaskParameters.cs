// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.FileMergeTaskParameters
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FreeYouTubeDownloader.Converter
{
    public class FileMergeTaskParameters
    {
        public string[] Input;
        public string Output;

        public string GetCommandLine()
        {
            return string.Format(Path.GetExtension(this.Output).ToLower().Equals(".webm") ? "-y {0} -c:v copy -f webm -c:a copy \"{1}\"" : "{0} -c:v copy -c:a copy -y -nostdin -f mp4 \"{1}\"", (object)((IEnumerable<string>)this.Input).Aggregate<string, string>(string.Empty, (Func<string, string, string>)((current, inputFile) => current + string.Format(" -i \"{0}\"", (object)inputFile))), (object)this.Output);
        }
    }
}
