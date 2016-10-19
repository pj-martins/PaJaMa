using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Diagnostics;

namespace PaJaMa.Common
{
	public class EncrypterDecrypter
	{
		private static EncrypterDecrypter _instance = null;
        private bool _cancel = false;
		public static EncrypterDecrypter Instance
		{
			get
			{
				if (_instance == null) _instance = new EncrypterDecrypter();
				return _instance;
			}
		}

		public EncrypterDecrypter()
		{
		}

		public event ProgressChangedEventHandler ProgressChanged;
        public event EventHandler Cancelled;

        [DebuggerNonUserCode()]
		public byte[] EncryptDecrypt(byte[] data, byte[] Key, byte[] IV, bool decrypt)
		{
			MemoryStream ms = new MemoryStream();
			Rijndael alg = Rijndael.Create();
			alg.Key = Key;
			alg.IV = IV;
			CryptoStream cs = new CryptoStream(ms,
			   (decrypt ? alg.CreateDecryptor() : alg.CreateEncryptor()), CryptoStreamMode.Write);
			cs.Write(data, 0, data.Length);
			cs.Close();
			byte[] returnData = ms.ToArray();

			return returnData;
		}

        public void Cancel()
        {
            _cancel = true;
        }

		public string Encrypt(string clearText, string Password)
		{
			byte[] clearBytes =
			  System.Text.Encoding.Unicode.GetBytes(clearText);

			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
				new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

			byte[] encryptedData = EncryptDecrypt(clearBytes,
					 pdb.GetBytes(32), pdb.GetBytes(16), false);

			return Convert.ToBase64String(encryptedData);

		}

        [DebuggerNonUserCode()]
		public string Decrypt(string cipherText, string Password)
		{
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
				new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
			byte[] decryptedData = EncryptDecrypt(cipherBytes,
				pdb.GetBytes(32), pdb.GetBytes(16), true);
			return System.Text.Encoding.Unicode.GetString(decryptedData);
		}

		public void EncryptDecryptFile(Stream srcStream, Stream destStream, string password, bool decrypt)
		{
			CryptoStream cs = null;
			try
			{
				Rijndael cryptic = Rijndael.Create();
				PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
					new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
				cryptic.Key = pdb.GetBytes(32);
				cryptic.IV = pdb.GetBytes(16);
				cs = new CryptoStream(destStream,
			   (decrypt ? cryptic.CreateDecryptor() : cryptic.CreateEncryptor()), CryptoStreamMode.Write);
				int data;
				double prog = 0;
				int progTot = 0;
				while ((data = srcStream.ReadByte()) != -1)
				{
                    if (_cancel)
                    {
                        cs.Close();
                        cs.Dispose();
                        if (Cancelled != null)
                        {
                            Cancelled(this, new EventArgs());
                        }
                        return;
                    }
					cs.WriteByte((byte)data);
					if (ProgressChanged != null)
					{
						prog++;
						if (Math.Floor((100 * prog) / srcStream.Length) > progTot)
						{
							progTot = Convert.ToInt16((100 * prog) / srcStream.Length);
							ProgressChanged(this, new ProgressChangedEventArgs(progTot, 
							null));
						}
					}
				}
			}
			finally
			{
				cs.Close();
				cs.Dispose();
			}
		}

		public void EncryptFile(string srcFile, string destFile, string password)
		{
			FileStream destStream = new FileStream(destFile, FileMode.Create);
			FileStream srcStream = new FileStream(srcFile, FileMode.Open);
			EncryptDecryptFile(srcStream, destStream, password, false);
			destStream.Close();
			srcStream.Close();
		}

		public void EncryptFile(string srcFile, Stream destStream, string password)
		{
			FileStream srcStream = new FileStream(srcFile, FileMode.Open);
			EncryptDecryptFile(srcStream, destStream, password, false);
			srcStream.Close();
		}

		public void EncryptFile(Stream srcStream, string destFile, string password)
		{
			FileStream destStream = new FileStream(destFile, FileMode.Create);
			EncryptDecryptFile(srcStream, destStream, password, false);
			destStream.Close();
		}
		public void EncryptFile(Stream srcStream, Stream destStream, string password)
		{
			EncryptDecryptFile(srcStream, destStream, password, false);
		}

		public void DecryptFile(string srcFile, string destFile, string password)
		{
			FileStream destStream = new FileStream(destFile, FileMode.Create);
			FileStream srcStream = new FileStream(srcFile, FileMode.Open);
			EncryptDecryptFile(srcStream, destStream, password, true);
			destStream.Close();
			srcStream.Close();
		}

		public void DecryptFile(string srcFile, Stream destStream, string password)
		{
			FileStream srcStream = new FileStream(srcFile, FileMode.Open);
			EncryptDecryptFile(srcStream, destStream, password, true);
			srcStream.Close();
		}

		public void DecryptFile(Stream srcStream, string destFile, string password)
		{
			FileStream destStream = new FileStream(destFile, FileMode.Create);
			EncryptDecryptFile(srcStream, destStream, password, true);
			destStream.Close();
		}
		public void DecryptFile(Stream srcStream, Stream destStream, string password)
		{
			EncryptDecryptFile(srcStream, destStream, password, true);
		}
	}


}
