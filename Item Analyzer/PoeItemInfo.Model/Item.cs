using System.Linq;

namespace PoeItemInfo.Model
{
	public class Item
	{
		public Data.Model.JSonProxy.Item Original { get; set; }
		
		public string Name { get; set; }
		public string TypeLine { get; set; }
		public bool Identified { get; set; }
		public bool Corrupted { get; set; }
		public int Quality { get; set; }
	}
}