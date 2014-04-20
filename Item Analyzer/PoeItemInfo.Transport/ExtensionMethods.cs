using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace PoeItemInfo.Transport
{
	public static class ExtensionMethods
	{
		public static string UnWrap(this SecureString secret)
		{
			if (secret == null)
				throw new ArgumentNullException("secret");

			var ptr = Marshal.SecureStringToCoTaskMemUnicode(secret);
			try
			{
				return Marshal.PtrToStringUni(ptr);
			}
			finally
			{
				Marshal.ZeroFreeCoTaskMemUnicode(ptr);
			}
		}

		public static byte[] ReadAllBytes(this StreamReader reader)
		{
			var bytes = new List<byte>();
			var buffer = new byte[1024];

			var readBytes = -1;
			while (readBytes != 0)
			{
				readBytes = reader.BaseStream.Read(buffer, 0, buffer.Length);
				bytes.AddRange(buffer.Take(readBytes));
			}

			return bytes.ToArray();
		}

		public static SecureString ToSecurestiSecureString(this string text)
		{
			var secureString = new SecureString();
			text.ToCharArray().ToList().ForEach(secureString.AppendChar);
			return secureString;
		}
	}
}