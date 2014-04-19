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
		public void ShouldParseOriginal()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.Build();
			
			var item = new ItemParser().Parse(original);
			item.Original.Should().BeSameAs(original);
		}

		[Fact]
		public void ShouldParseCategory()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.name = "Marohi Erqi")
				.Build();

			var parsedItem = new ItemParser().Parse(original);
			parsedItem.Name.Should().Be("Marohi Erqi");
		}

		[Fact]
		public void ShouldParseTypeline()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.typeLine = "Karui Maul")
				.Build();

			var parsedItem = new ItemParser().Parse(original);
			parsedItem.TypeLine.Should().Be("Karui Maul");
		}

		[Fact]
		public void ShouldParseIdentified()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.identified = true)
				.Build();

			var parsedItem = new ItemParser().Parse(original);
			parsedItem.Identified.Should().BeTrue();
		}

		[Fact]
		public void ShouldParseCorrupted()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.corrupted = true)
				.Build();

			var parsedItem = new ItemParser().Parse(original);
			parsedItem.Corrupted.Should().BeTrue();
		}

		[Fact]
		public void ShouldParseQuality()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.Quality(17)
				.Build();

			var parsedItem = new ItemParser().Parse(original);
			parsedItem.Quality.Should().Be(17);
		}


	}
}