using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace POEApi.Infrastructure
{
	public static class Extensions
	{
		public static T GetEnum<T>(this XAttribute attribute)
		{
			return (T) Enum.Parse(typeof (T), attribute.Value);
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
	}
}