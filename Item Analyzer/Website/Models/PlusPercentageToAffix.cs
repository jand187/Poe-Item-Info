using System.Linq;

namespace Website.Models
{
	public class PlusPercentageToAffix : Affix
	{
		public override string Name { get; set; }
		public override int Value { get; set; }

		public override string ToString()
		{
			return string.Format("+{0}% to {1} ({2})", Value, Name, this.GetType().Name);
		}
	}
}