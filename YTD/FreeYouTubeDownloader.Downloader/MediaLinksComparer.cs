// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.MediaLinksComparer
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System.Collections.Generic;

namespace FreeYouTubeDownloader.Downloader
{
    public class MediaLinksComparer : EqualityComparer<MediaLink>
    {
        public override bool Equals(MediaLink x, MediaLink y)
        {
            return x.Equals((object)y);
        }

        public override int GetHashCode(MediaLink obj)
        {
            if (obj == null)
                return 0;
            return obj.GetHashCode();
        }
    }
}
