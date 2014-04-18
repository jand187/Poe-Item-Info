using System.Collections.Generic;
using System.Linq;

namespace PoeItemInfo.Data.Model.JSonProxy
{
	public class Requirement
	{
		public string name { get; set; }
		public IEnumerable<IEnumerable<object>> values { get; set; }
		public int displayMode { get; set; }
	}
}