using System.Linq;

namespace Website.Models
{
	public class PercentIncreaseAffix : Affix
	{
		public string Name { get; set; }
		public string Value { get; set; }

		public override string ToString()
		{
			return string.Format("+{0} to {1} ({2})", Value, Name, this.GetType().Name);
		}
	}
}