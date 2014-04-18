using System.Linq;

namespace Website.Models
{
	public class UnknownAffix : Affix
	{
		public string Text { get; set; }

		public override string ToString()
		{
			return string.Format("{0} ({1})", Text, this.GetType().Name);
		}

		public override string Name
		{
			get { return string.Empty; }

			set { throw new System.NotImplementedException(); }
		}

		public override string Style
		{
			get { return "unknown-affix"; }
		}

		public override int Value { get; set; }
	}
}