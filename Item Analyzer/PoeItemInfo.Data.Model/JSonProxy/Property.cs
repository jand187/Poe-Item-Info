using System.Collections.Generic;
using System.Linq;

namespace PoeItemInfo.Data.Model.JSonProxy
{
	public class Property
	{
		public string name { get; set; }
		public IEnumerable<object> values { get; set; }
		public int displayMode { get; set; }
	}
}