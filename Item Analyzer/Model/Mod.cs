using System.Collections.Generic;

namespace Model
{
	public class BaseMod
	{
		public IEnumerable<Mod> base_mods { get; set; }
	}

	public class Mod
	{
		public string mod_id { get; set; }
		public List<Range<int>> Ranges { get; set; }
	}

	public class Range<TType>
	{
		public TType Max { get; set; }
		public TType Min { get; set; }
	}
}
