using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security;
using POEApi.Model.JSONProxy;
using POEApi.Transport;

namespace POEApi.Model
{
	public class POEModel
	{
		private ITransport transport;

		public bool Authenticate(string email, SecureString password, bool offline, bool useSessionId)
		{
			transport = new HttpTransport(email);
			transport.Authenticate(email, password, useSessionId);

			return true;
		}

		public Stash GetStash(int index, string league, bool forceRefresh)
		{
			var serialiser = new DataContractJsonSerializer(typeof (JSONProxy.Stash));
			JSONProxy.Stash proxy = null;

			using (var stream = transport.GetStash(index, league, forceRefresh))
				proxy = (JSONProxy.Stash) serialiser.ReadObject(stream);

			return new Stash(proxy);
		}


		public List<Character> GetCharacters()
		{
			var serialiser = new DataContractJsonSerializer(typeof (List<JSONProxy.Character>));
			List<JSONProxy.Character> characters;

			using (var stream = transport.GetCharacters())
				characters = (List<JSONProxy.Character>) serialiser.ReadObject(stream);

			return characters.Select(c => new Character(c)).ToList();
		}

		public List<Item> GetInventory(string characterName)
		{
			var serialiser = new DataContractJsonSerializer(typeof (Inventory));
			Inventory item;

			using (var stream = transport.GetInventory(characterName))
				item = (Inventory) serialiser.ReadObject(stream);

			return item.Items == null ? new List<Item>() : item.Items.Select(ItemFactory.Get).ToList();
		}
	}
}