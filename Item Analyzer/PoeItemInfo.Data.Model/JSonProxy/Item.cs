using System.Collections.Generic;
using System.Linq;

namespace PoeItemInfo.Data.Model.JSonProxy
{
	public class Item
	{
		public bool verified { get; set; }
		public int w { get; set; }
		public int h { get; set; }
		public string icon { get; set; }
		public bool support { get; set; }
		public string league { get; set; }
		public IEnumerable<Socket> sockets { get; set; }
		public string name { get; set; }
		public string typeLine { get; set; }
		public bool identified { get; set; }
		public bool corrupted { get; set; }
		public IEnumerable<Property> properties { get; set; }
		public IEnumerable<Requirement> requirements { get; set; }
		public IEnumerable<string> implicitMods { get; set; }
		public IEnumerable<string> explicitMods { get; set; }
		public int frameType { get; set; }
		public int x { get; set; }
		public int y { get; set; }
		public string inventoryId { get; set; }
		public IEnumerable<Item> socketedItems { get; set; }
	}
}