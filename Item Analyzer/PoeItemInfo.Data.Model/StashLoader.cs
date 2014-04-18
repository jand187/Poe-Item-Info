using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PoeItemInfo.Data.Model.JSonProxy;

namespace PoeItemInfo.Data.Model
{
	public class StashLoader
	{
		public Stash Load(Stream stream)
		{
			using (var reader = new StreamReader(stream))
				return Load(reader.ReadToEnd());
		}

		public Stash Load(string text)
		{
			return JsonConvert.DeserializeObject<Stash>(text);
		}
	}
}