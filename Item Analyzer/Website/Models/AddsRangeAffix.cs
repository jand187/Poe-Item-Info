using System;
using System.Linq;

namespace Website.Models
{
	public class AddsRangeAffix : Affix
	{
		public override string Name { get; set; }
		public int Low { get; set; }
		public int High { get; set; }

		public override int Value
		{
			get { return (Low + High)/2; }
			set { throw new InvalidOperationException("No setter."); }
		}

		public override string ToString()
		{
			return string.Format("Adds {0}-{1} {2} ({3})", Low, High, Name, this.GetType().Name);
		}
	}
}