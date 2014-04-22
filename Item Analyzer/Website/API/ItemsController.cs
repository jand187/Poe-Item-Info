using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using PoeItemInfo.Data.Model.JSonProxy;
using PoeItemInfo.Model;
using Website.Properties;

namespace Website.API
{
	public class ItemsController : ApiController
	{
		private readonly IItemParser itemParser;

		public ItemsController(IItemParser itemParser)
		{
			this.itemParser = itemParser;
		}

		public ItemsController() 
		{
			var typeCategoryMap = LoadStuff();

			var propertyParser = new PropertyParser();
			var requirementParser = new RequirementParser();
			var modsParser = new ModsParser();
			var itemTypeParser = new ItemTypeParser(typeCategoryMap);
			this.itemParser = new ItemParser(propertyParser, requirementParser, modsParser, itemTypeParser);
		}

		private IEnumerable<ItemType> LoadStuff()
		{
			//var itemTypes = new[]
			//{
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Alchemy Shard"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Alteration Shard"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Transmutation Shard"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Scroll Fragment"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Scroll of Wisdom"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Portal Scroll"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Orb of Augmentation"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Orb of Transmutation"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Orb of Alteration"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Orb of Chance"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Orb of Fusing"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Orb of Scouring"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Orb of Regret"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Orb of Alchemy"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Regal Orb"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Blessed Orb"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Chromatic Orb"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Jeweller's Orb"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Vaal Orb"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Gemcutter's Prism"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Albino Rhoa Feather"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Cartographer's Chisel"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Glassblower's Bauble"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Blacksmith's Whetstone"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Currency", Type = "Armourer's Scrap"},

			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Vaal", Type = "Sacrifice at Dawn"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Vaal", Type = "Sacrifice at Dusk"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Vaal", Type = "Sacrifice at Midnight"},
			//	new ItemType {Category = ItemCategories.Currency, BaseType = "Vaal", Type = "Sacrifice at XXX"},

			//	new ItemType {Category = ItemCategories.Weapon, BaseType = "Wand", Type = "Carved Wand"},
			//	new ItemType {Category = ItemCategories.Weapon, BaseType = "Wand", Type = "Engraved Wand"},

			//	new ItemType {Category = ItemCategories.Weapon, BaseType = "Dagger", Type = "Copper Kris"},
			//	new ItemType {Category = ItemCategories.Weapon, BaseType = "Dagger", Type = "Golden Kris"},
			//	new ItemType {Category = ItemCategories.Weapon, BaseType = "Dagger", Type = "Platinum Kris"},
			//	new ItemType {Category = ItemCategories.Weapon, BaseType = "Dagger", Type = "Imp Dagger"},

			//	new ItemType {Category = ItemCategories.Weapon, BaseType = "Bow", Type = "Sniper Bow"},

			//	new ItemType {Category = ItemCategories.Armour, BaseType = "Helmet", Type = "Noble Tricorne"},
			//	new ItemType {Category = ItemCategories.Armour, BaseType = "Helmet", Type = "Callous Mask"},

			//	new ItemType {Category = ItemCategories.Armour, BaseType = "Body armour", Type = "Hussar Brigandine"},

			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Amethyst Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Paua Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Gold Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Topaz Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Prismatic Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Sapphire Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Two-Stone Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Diamond Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Coral Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Ruby Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Moonstone Ring"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Ring", Type = "Iron Ring"},

			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Amulet", Type = "Paua Amulet"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Amulet", Type = "Onyx Amulet"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Amulet", Type = "Lapis Amulet"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Amulet", Type = "Agate Amulet"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Amulet", Type = "Gold Amulet"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Amulet", Type = "Amber Amulet"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Amulet", Type = "Jade Amulet"},
			//	new ItemType {Category = ItemCategories.Jewelery, BaseType = "Amulet", Type = "Citrine Amulet"},

			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Anger"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Animate Guardian"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Cleave"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Decoy Totem"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Determination"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Devouring Totem"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Dominating Blow"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Enduring Cry"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Flame Totem"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Glacial Hammer"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Ground Slam"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Heavy Strike"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Immortal Call"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Infernal Blow"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Leap Slam"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Lightning Strike"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Molten Shell"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Punishment"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Purity of Fire"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Rejuvenation Totem"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Searing Bond"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Shield Charge"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Shockwave Totem"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Sweep"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Vitality"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Warlord's Mark"},

			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Animate Weapon"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Arctic Armour"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Barrage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Bear Trap"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Blood Rage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Burning Arrow"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Cyclone"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Desecrate"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Detonate Dead"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Double Strike"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Dual Strike"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Elemental Hit"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Ethereal Knives"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Explosive Arrow"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Fire Trap"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Flicker Strike"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Freeze Mine"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Frenzy"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Grace"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Haste"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Hatred"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Ice Shot"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Lightning Arrow"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Poison Arrow"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Projectile Weakness"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Puncture"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Purity of Ice"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Rain of Arrows"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Reave"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Smoke Mine"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Spectral Throw"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Split Arrow"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Temporal Chains"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Viper Strike"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Whirling Blades"},

			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Arc"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Arctic Breath"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Bone Offering"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Clarity"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Cold Snap"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Conductivity"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Conversion Trap"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Critical Weakness"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Discharge"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Discipline"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Elemental Weakness"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Enfeeble"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Fireball"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Firestorm"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Flameblast"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Flammability"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Flesh Offering"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Freezing Pulse"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Frost Wall"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Frostbite"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Glacial Cascade"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Ice Nova"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Ice Spear"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Incinerate"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Lightning Trap"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Lightning Warp"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Power Siphon"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Purity of Elements"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Purity of Lightning"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Raise Spectre"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Raise Zombie"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Righteous Fire"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Shock Nova"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Spark"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Storm Call"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Summon Raging Spirit"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Summon Skeletons"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Tempest Shield"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Vulnerability"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem", Type = "Wrath"},

			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Added Fire Damage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Blood Magic"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Cast on Melee Kill"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Cast when Damage Taken"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Cold to Fire"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Empower"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Endurance Charge on Melee Stun"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Fire Penetration"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Increased Burning Damage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Increased Duration"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Iron Grip"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Iron Will"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Item Quantity"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Knockback"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Life Gain on Hit"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Life Leech"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Melee Damage on Full Life"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Melee Physical Damage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Melee Splash"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Multistrike"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Ranged Attack Totem"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Reduced Duration"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Reduced Mana"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Spell Totem"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Stun"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Weapon Elemental Damage"},

			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Added Cold Damage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Additional Accuracy"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Blind"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Cast on Critical Strike"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Cast on Death"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Chain"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Chance to Flee"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Cold Penetration"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Culling Strike"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Enhance"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Faster Attacks"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Faster Projectiles"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Fork"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Greater Multiple Projectiles"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Lesser Multiple Projectiles"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Mana Leech"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Multiple Traps"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Physical Projectile Attack Damage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Pierce"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Point Blank"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Slower Projectiles"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Trap"},

			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Portal"},

			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Added Chaos Damage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Added Lightning Damage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Cast when Stunned"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Chance to Ignite"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Concentrated Effect"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Curse on Hit"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Elemental Proliferation"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Enlighten"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Faster Casting"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Increased Area of Effect"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Increased Critical Damage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Increased Critical Strikes"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Item Rarity"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Lightning Penetration"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Minion and Totem Elemental Resistance"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Minion Damage"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Minion Life"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Minion Speed"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Power Charge On Critical"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Support", Type = "Remote Mine"},

			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Vaal", Type = "Vaal Ground Slam"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Vaal", Type = "Vaal Glacial Hammer"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Vaal", Type = "Vaal Lightning Warp"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Vaal", Type = "Vaal Double Strike"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Vaal", Type = "Vaal Immortal Call"},
			//	new ItemType {Category = ItemCategories.SkillGem, BaseType = "SkillGem Vaal", Type = "Vaal Ground Slamxx"},
			//};

			var filename = Path.Combine(Settings.Default.DataBaseDirectory, "categories.json");
			var contents = File.ReadAllText(filename);

			return JsonConvert.DeserializeObject<IEnumerable<ItemType>>(contents);
		}

		public dynamic Get(ItemCategories category)
		{
			var stashFiles = Directory.GetFiles(Settings.Default.DataDirectory, "*.json");

			var list = stashFiles
				.Select(File.ReadAllText)
				.Select(JsonConvert.DeserializeObject<Stash>)
				.SelectMany(stash => itemParser.Parse(stash));


			return new
			{
				Success = true,
				Items = list.Where(i => i.Category == category.ToString()),
			};
		}

		public dynamic Get()
		{
			var stashFiles = Directory.GetFiles(Settings.Default.DataDirectory, "*.json");

			var list = stashFiles
				.Select(File.ReadAllText)
				.Select(JsonConvert.DeserializeObject<Stash>)
				.SelectMany(stash => itemParser.Parse(stash));


			return new
			{
				Success = true,
				Items = list,
			};
		}



	}
}