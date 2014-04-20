using System.Linq;
using FluentAssertions;
using NSubstitute;
using PoeItemInfo.Data.Model.JSonProxy;
using PoeItemInfo.Model;
using TestCommon;
using Xunit;
using Item = PoeItemInfo.Data.Model.JSonProxy.Item;

namespace PoeItemInfo.ModelTest
{
	public class ItemParserTest
	{
		private readonly ItemParser itemParser;
		private readonly IPropertyParser propertyParserMock;
		private readonly IRequirementParser requirementParserMock;
		private readonly IModsParser modsParserMock;

		public ItemParserTest()
		{
			propertyParserMock = Substitute.For<IPropertyParser>();
			requirementParserMock = Substitute.For<IRequirementParser>();
			modsParserMock = Substitute.For<IModsParser>();
			itemParser = new ItemParser(propertyParserMock, requirementParserMock, modsParserMock);
		}

		[Fact]
		public void ShouldParseOriginal()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.Build();

			var item = itemParser.Parse(original);
			item.Original.Should().BeSameAs(original);
		}

		[Fact]
		public void ShouldParseCategory()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.name = "Marohi Erqi")
				.Build();

			var parsedItem = itemParser.Parse(original);
			parsedItem.Name.Should().Be("Marohi Erqi");
		}

		[Fact]
		public void ShouldParseTypeline()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.typeLine = "Karui Maul")
				.Build();

			var parsedItem = itemParser.Parse(original);
			parsedItem.TypeLine.Should().Be("Karui Maul");
		}

		[Fact]
		public void ShouldParseIdentified()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.identified = true)
				.Build();

			var parsedItem = itemParser.Parse(original);
			parsedItem.Identified.Should().BeTrue();
		}

		[Fact]
		public void ShouldParseCorrupted()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.corrupted = true)
				.Build();

			var parsedItem = itemParser.Parse(original);
			parsedItem.Corrupted.Should().BeTrue();
		}

		[Fact]
		public void ShouldParseProperties()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.properties = new Property[0])
				.Build();

			var parsedItem = itemParser.Parse(original);
			propertyParserMock.Received().Parse(original.properties);
			parsedItem.Properties.Should().NotBeNull();
		}

		[Fact]
		public void ShouldParseRequirement()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.requirements = new Requirement[0])
				.Build();

			var parsedItem = itemParser.Parse(original);
			requirementParserMock.Received().Parse(original.requirements);
			parsedItem.Requirements.Should().NotBeNull();
		}

		[Fact]
		public void ShouldParseExplicitMods()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.explicitMods = new string[0])
				.Build();

			var parsedItem = itemParser.Parse(original);
			modsParserMock.Received().Parse(original.explicitMods);
			parsedItem.ExplicitMods.Should().NotBeNull();
		}

		[Fact]
		public void ShouldParseImplicitMods()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.implicitMods = new string[0])
				.Build();

			var parsedItem = itemParser.Parse(original);
			modsParserMock.Received().Parse(original.implicitMods);
			parsedItem.ImplicitMods.Should().NotBeNull();
		}

		[Fact]
		public void ShouldParseLocation()
		{
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.x = 3)
				.With(item => item.y = 5)
				.With(item => item.inventoryId = "Stash51")
				.Build();

			var parsedItem = itemParser.Parse(original);
			parsedItem.Location.Should().NotBeNull();
		}

		[Fact]
		public void ShouldParseSocketedItems()
		{
			var name = "some item";
			var original = new GenericBuilder<Item>()
				.Item()
				.With(item => item.socketedItems = new[]
				{
					new Item
					{
						name = name
					}
				})
				.Build();

			var parsedItem = itemParser.Parse(original);
			parsedItem.SocketedItems.First().Name.Should().Be(name);
		}
	}
}