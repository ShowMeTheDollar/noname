using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Extensions
{
	public static class EncryptDecryptExtension
	{
		private static readonly string Key = "1234567890abcdef1234567890abcdef"; 

		public static string Encrypt(this string textToEncrypt)
		{
			try
			{
				byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key);
				byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(textToEncrypt);

				using (AesManaged aes = new AesManaged())
				{
					aes.Key = keyArray;
					aes.Mode = CipherMode.ECB;
					aes.Padding = PaddingMode.PKCS7;

					ICryptoTransform cTransform = aes.CreateEncryptor();
					byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

					return Convert.ToBase64String(resultArray, 0, resultArray.Length);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("加密過程出現錯誤: " + ex.Message);
			}
		}

		public static string Decrypt(this string textToDecrypt)
		{
			try
			{
				byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key);
				byte[] toDecryptArray = Convert.FromBase64String(textToDecrypt);

				using (AesManaged aes = new AesManaged())
				{
					aes.Key = keyArray;
					aes.Mode = CipherMode.ECB;
					aes.Padding = PaddingMode.PKCS7;

					ICryptoTransform cTransform = aes.CreateDecryptor();
					byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

					return UTF8Encoding.UTF8.GetString(resultArray);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("解密過程出現錯誤: " + ex.Message);
			}
		}
	}
}
