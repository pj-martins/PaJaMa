// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.ZipUtil
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.IO;
using System.Text;

namespace FreeYouTubeDownloader.Downloader
{
  public static class ZipUtil
  {
    public static byte[] Zip(string str)
    {
      using (ZipFile zipFile = new ZipFile())
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          zipFile.CompressionLevel = CompressionLevel.BestCompression;
          zipFile.AddEntry("stream.zip", str, Encoding.UTF8);
          zipFile.Save((Stream) memoryStream);
          return memoryStream.ToArray();
        }
      }
    }

    public static string Unzip(byte[] bytes)
    {
      using (MemoryStream memoryStream1 = new MemoryStream(bytes))
      {
        using (ZipFile zipFile = ZipFile.Read((Stream) memoryStream1))
        {
          ZipEntry zipEntry = zipFile["stream.zip"];
          using (MemoryStream memoryStream2 = new MemoryStream(Convert.ToInt32(zipEntry.UncompressedSize)))
          {
            zipEntry.Extract((Stream) memoryStream2);
            return Encoding.UTF8.GetString(memoryStream2.ToArray());
          }
        }
      }
    }

    public static string Unzip(Stream inputStream)
    {
      using (ZipFile zipFile = ZipFile.Read(inputStream))
      {
        ZipEntry zipEntry = zipFile["stream.zip"];
        using (MemoryStream memoryStream = new MemoryStream(Convert.ToInt32(zipEntry.UncompressedSize)))
        {
          zipEntry.Extract((Stream) memoryStream);
          return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
      }
    }
  }
}
