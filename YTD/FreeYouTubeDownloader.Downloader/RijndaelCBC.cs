// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.RijndaelCBC
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace FreeYouTubeDownloader.Downloader
{
  internal class RijndaelCBC
  {
    private readonly int _keyBitSize;
    private BufferedBlockCipher _cipher;
    private readonly KeyParameter _key;
    private ParametersWithIV _vkey;

    public byte[] Iv { get; private set; }

    internal RijndaelCBC(int keySize, byte[] salt, string passphrase)
    {
      this._keyBitSize = keySize;
      Random random = new Random();
      this.Iv = new byte[this._keyBitSize / 8];
      for (int index = 0; index < this._keyBitSize / 8; ++index)
        random.Next();
      random.NextBytes(this.Iv);
      PbeParametersGenerator parametersGenerator = (PbeParametersGenerator) new Pkcs5S2ParametersGenerator();
      parametersGenerator.Init(PbeParametersGenerator.Pkcs5PasswordToUtf8Bytes(passphrase.ToCharArray()), salt, 1000);
      this._key = (KeyParameter) parametersGenerator.GenerateDerivedParameters("AES", this._keyBitSize);
    }

    internal RijndaelCBC(int keySize, byte[] salt, string passphrase, byte[] iv)
    {
      this._keyBitSize = keySize;
      this.Iv = iv;
      PbeParametersGenerator parametersGenerator = (PbeParametersGenerator) new Pkcs5S2ParametersGenerator();
      parametersGenerator.Init(PbeParametersGenerator.Pkcs5PasswordToUtf8Bytes(passphrase.ToCharArray()), salt, 1000);
      this._key = (KeyParameter) parametersGenerator.GenerateDerivedParameters("AES", this._keyBitSize);
    }

    internal byte[] Encrypt(byte[] data)
    {
      this._cipher = (BufferedBlockCipher) new PaddedBufferedBlockCipher((IBlockCipher) new CbcBlockCipher((IBlockCipher) new RijndaelEngine(this._keyBitSize)));
      this._vkey = new ParametersWithIV((ICipherParameters) this._key, this.Iv);
      this._cipher.Init(true, (ICipherParameters) this._vkey);
      int outputSize = this._cipher.GetOutputSize(data.Length);
      byte[] output = new byte[outputSize];
      int outOff = this._cipher.ProcessBytes(data, 0, data.Length, output, 0);
      int length = outOff + this._cipher.DoFinal(output, outOff);
      if (length < outputSize)
      {
        byte[] numArray = new byte[length];
        Array.Copy((Array) output, 0, (Array) numArray, 0, length);
        output = numArray;
      }
      return output;
    }

    internal byte[] Decrypt(byte[] data)
    {
      this._cipher = (BufferedBlockCipher) new PaddedBufferedBlockCipher((IBlockCipher) new CbcBlockCipher((IBlockCipher) new RijndaelEngine(this._keyBitSize)));
      this._vkey = new ParametersWithIV((ICipherParameters) this._key, this.Iv);
      this._cipher.Init(false, (ICipherParameters) this._vkey);
      int outputSize = this._cipher.GetOutputSize(data.Length);
      byte[] output = new byte[outputSize];
      int outOff = this._cipher.ProcessBytes(data, 0, data.Length, output, 0);
      int length = outOff + this._cipher.DoFinal(output, outOff);
      if (length < outputSize)
      {
        byte[] numArray = new byte[length];
        Array.Copy((Array) output, 0, (Array) numArray, 0, length);
        output = numArray;
      }
      return output;
    }
  }
}
