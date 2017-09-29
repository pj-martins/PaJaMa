// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Authorization
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace FreeYouTubeDownloader.Downloader
{
  internal static class Authorization
  {
    private const string Key = "x1(V2%yt+A!,GJq,";
    private const string Passphrase = "gez5spe8wa$@spep";
    private const string TokenBase = "DLKJFNI&W347JDH";

    public static bool CheckToken(string hash)
    {
      string end;
      using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
      {
        rijndaelManaged.Key = Encoding.ASCII.GetBytes("x1(V2%yt+A!,GJq,");
        rijndaelManaged.IV = Encoding.ASCII.GetBytes("gez5spe8wa$@spep");
        ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
        using (MemoryStream memoryStream = new MemoryStream(Authorization.StringToByteArray(hash)))
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
              end = streamReader.ReadToEnd();
          }
        }
      }
      return end.StartsWith("DLKJFNI&W347JDH");
    }

    public static string GenerateToken()
    {
      DateTime dateTime;
      try
      {
        dateTime = Authorization.GetNetworkTime(true);
      }
      catch
      {
        dateTime = DateTime.UtcNow;
      }
      string str = "DLKJFNI&W347JDH" + ":" + (object) (int) dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
      byte[] array;
      using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
      {
        rijndaelManaged.IV = Encoding.ASCII.GetBytes("gez5spe8wa$@spep");
        rijndaelManaged.KeySize = 128;
        rijndaelManaged.Key = Encoding.ASCII.GetBytes("x1(V2%yt+A!,GJq,");
        ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream))
              streamWriter.Write(str);
            array = memoryStream.ToArray();
          }
        }
      }
      return Authorization.ByteArrayString(array);
    }

    private static string ByteArrayString(byte[] bytes)
    {
      StringBuilder stringBuilder = new StringBuilder(bytes.Length);
      foreach (byte num in bytes)
        stringBuilder.AppendFormat("{0:x2}", (object) num);
      return stringBuilder.ToString();
    }

    private static byte[] StringToByteArray(string hex)
    {
      return Enumerable.Range(0, hex.Length).Where<int>((Func<int, bool>) (x => x % 2 == 0)).Select<int, byte>((Func<int, byte>) (x => Convert.ToByte(hex.Substring(x, 2), 16))).ToArray<byte>();
    }

    public static DateTime GetNetworkTime(bool utc)
    {
      byte[] numArray = new byte[48];
      numArray[0] = (byte) 27;
      IPEndPoint ipEndPoint1 = new IPEndPoint(Dns.GetHostEntry("time.windows.com").AddressList[0], 123);
      Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      IPEndPoint ipEndPoint2 = ipEndPoint1;
      socket.Connect((EndPoint) ipEndPoint2);
      int num1 = 3000;
      socket.ReceiveTimeout = num1;
      byte[] buffer1 = numArray;
      socket.Send(buffer1);
      byte[] buffer2 = numArray;
      socket.Receive(buffer2);
      socket.Close();
      long uint32_1 = (long) BitConverter.ToUInt32(numArray, 40);
      ulong uint32_2 = (ulong) BitConverter.ToUInt32(numArray, 44);
      long num2 = (long) Authorization.SwapEndianness((ulong) uint32_1);
      ulong num3 = (ulong) Authorization.SwapEndianness(uint32_2);
      long num4 = 1000;
      DateTime dateTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double) (long) ((ulong) (num2 * num4) + num3 * 1000UL / 4294967296UL));
      if (!utc)
        return dateTime.ToLocalTime();
      return dateTime;
    }

    private static uint SwapEndianness(ulong x)
    {
      return (uint) ((ulong) ((((long) x & (long) byte.MaxValue) << 24) + (((long) x & 65280L) << 8)) + ((x & 16711680UL) >> 8) + ((x & 4278190080UL) >> 24));
    }
  }
}
