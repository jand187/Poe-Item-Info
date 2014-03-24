using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Model;
using Newtonsoft.Json;
using Xunit;

namespace ModelTest
{
	public class ModParsingTest
	{
		[Fact]
		public void WhatDoesItTest()
		{
			var file = LoadTextFile(@"TestData\Mods.json");
			var mods = JsonConvert.DeserializeObject<BaseMod>(file);
			
			mods.Should().NotBeNull();
			mods.base_mods.Count().Should().Be(2);
		}

		private string LoadTextFile(string file)
		{
			using (var stream = File.OpenRead(file))
			{
				using (var reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}
	}
}
