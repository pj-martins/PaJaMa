// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Extensions
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

namespace FreeYouTubeDownloader.Downloader
{
  public static class Extensions
  {
    public static T[] Slice<T>(this T[] source, int start, int end)
    {
      if (end < 0)
        end = source.Length + end;
      int length = end - start;
      T[] objArray = new T[length];
      for (int index = 0; index < length; ++index)
        objArray[index] = source[index + start];
      return objArray;
    }

    public static T[] Slice<T>(this T[] source, int start)
    {
      int num = 0;
      int length = source.Length + num - start;
      T[] objArray = new T[length];
      for (int index = 0; index < length; ++index)
        objArray[index] = source[index + start];
      return objArray;
    }
  }
}
