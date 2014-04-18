using System.Linq;

namespace Website.Models
{
	public class ReflectAffix : Affix
	{
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public override int Value { get; set; }

		public override string ToString()
		{
			return string.Format("{2} {0} {1} ({3})", Value, Name2, Name1, this.GetType().Name);
		}

		public override string Name
		{
			get { return string.Format("{0} {1}", Name1, Name2); }
			set { }
		}
	}
}