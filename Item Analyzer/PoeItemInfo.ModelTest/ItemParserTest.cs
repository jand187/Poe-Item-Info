using System.Linq;
using FluentAssertions;
using PoeItemInfo.Model;
using TestCommon;
using Xunit;
using Item = PoeItemInfo.Data.Model.JSonProxy.Item;

namespace PoeItemInfo.ModelTest
{
	public class ItemParserTest
	{
		[Fact]
		public void ShouldParseAxeType()
		{
			var build = new GenericBuilder<Item>()
				.OneHandedAxe()
				.Build();

			var axe = new ItemParser().Parse(build);
			axe.Type.Should().Be("One Handed Axe");
		}
	}
}