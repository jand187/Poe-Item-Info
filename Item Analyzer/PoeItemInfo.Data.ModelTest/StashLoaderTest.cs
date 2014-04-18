using System.Linq;
using FluentAssertions;
using PoeItemInfo.Data.Model;
using PoeItemInfo.Data.ModelTest.Properties;
using Xunit;

namespace PoeItemInfo.Data.ModelTest
{
	public class StashLoaderTest
	{
		[Fact]
		public void CanLoadJSonFromStream()
		{
			var stash = new StashLoader().Load(Resources.FullStashTab);

			stash.numTabs.Should().Be(88);
		}
	}
}