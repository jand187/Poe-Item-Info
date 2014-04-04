using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security;
using POEApi.Infrastructure;
using POEApi.Infrastructure.Events;
using POEApi.Model.Events;
using POEApi.Model.JSONProxy;
using POEApi.Transport;

namespace POEApi.Model
{
	public class POEModel
	{
		public delegate void AuthenticateEventHandler(POEModel sender, AuthenticateEventArgs e);

		public delegate void ImageLoadEventHandler(POEModel sender, ImageLoadedEventArgs e);

		public delegate void StashLoadEventHandler(POEModel sender, StashLoadedEventArgs e);

		private readonly bool downOnlyMyCharacters;
		private CacheService cacheService;
		private ITransport transport;

		public POEModel()
		{
			downOnlyMyCharacters = bool.Parse(Settings.UserSettings["DownloadOnlyMyCharacters"]);
		}

		public bool Offline { get; private set; }
		public event AuthenticateEventHandler Authenticating;

		public event StashLoadEventHandler StashLoading;

		public event ImageLoadEventHandler ImageLoading;

		public event ThottledEventHandler Throttled;

		public bool Authenticate(string email, SecureString password, bool offline, bool useSessionID)
		{
			transport = new HttpTransport(email); //new CachedTransport(email, new HttpTransport(email));
			cacheService = new CacheService(email);

			transport.Throttled += instance_Throttled;
			onAuthenticate(POEEventState.BeforeEvent, email);

			transport.Authenticate(email, password, useSessionID);

			onAuthenticate(POEEventState.AfterEvent, email);

			return true;
		}

		private void instance_Throttled(object sender, ThottledEventArgs e)
		{
			if (Throttled != null)
				Throttled(sender, e);
		}

		public void ForceRefresh()
		{
			cacheService.Clear();
		}

		public Stash GetStash(int index, string league, bool forceRefresh)
		{
			var serialiser = new DataContractJsonSerializer(typeof (JSONProxy.Stash));
			JSONProxy.Stash proxy = null;

			onStashLoaded(POEEventState.BeforeEvent, index, -1);

			using (var stream = transport.GetStash(index, league, forceRefresh))
			{
				try
				{
					proxy = (JSONProxy.Stash) serialiser.ReadObject(stream);
					if (proxy == null)
						logNullStash(stream, "Proxy was null");
				}
				catch (Exception ex)
				{
					Logger.Log(ex);
					logNullStash(stream, "JSON Serialization Failed");
				}
			}

			onStashLoaded(POEEventState.AfterEvent, index, proxy.NumTabs);

			return new Stash(proxy);
		}

		private void logNullStash(Stream stream, string errorPrefix)
		{
			try
			{
				var ms = stream as MemoryStream;
				ms.Seek(0, SeekOrigin.Begin);
				Logger.Log(errorPrefix + ": base64 bytes:");
				Logger.Log(Convert.ToBase64String(ms.ToArray()));
				Logger.Log("END");
			}
			catch (Exception ex)
			{
				Logger.Log(ex);
			}

			throw new Exception(@"Downloading stash, details logged to DebugLog.log, please open a ticket at https://code.google.com/p/procurement/issues/list");
		}

		public Stash GetStash(string league)
		{
			var myTabs = Settings.Lists["MyTabs"];
			var onlyMyTabs = myTabs.Count != 0;

			var stash = GetStash(0, league, false);

			var skippedTabs = new List<Tab>();

			if (!onlyMyTabs)
				return getAllTabs(league, stash);

			var tabCount = 0;

			for (var i = 1; i < stash.NumberOfTabs; i++)
			{
				if (myTabs.Contains(stash.Tabs[i].Name))
				{
					stash.Add(GetStash(i, league, false));
					++tabCount;
				}
				else
					skippedTabs.Add(stash.Tabs[i]);
			}

			foreach (var tab in skippedTabs)
				stash.Tabs.Remove(tab);

			stash.NumberOfTabs = tabCount + 1;

			return stash;
		}

		private Stash getAllTabs(string league, Stash stash)
		{
			for (var i = 1; i < stash.NumberOfTabs; i++)
				stash.Add(GetStash(i, league, false));

			return stash;
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
			try
			{
				if (downOnlyMyCharacters && !Settings.Lists["MyCharacters"].Contains(characterName))
					return new List<Item>();

				var serialiser = new DataContractJsonSerializer(typeof (Inventory));
				Inventory item;

				using (var stream = transport.GetInventory(characterName))
					item = (Inventory) serialiser.ReadObject(stream);

				if (item.Items == null)
					return new List<Item>();

				return item.Items.Select(i => ItemFactory.Get(i)).ToList();
			}
			catch (SerializationException sex)
			{
				Logger.Log(sex);
				throw new Exception(
					string.Format(
						"Error reading character data for {0}, if you are in offline mode you will need to login and update. If you received this error while logging in, the authenticated session may have expired or bad data has been returned by GGG or a network issue may have occurred - Please try again.",
						characterName));
			}
		}

		public void GetImages(Stash stash)
		{
			foreach (var item in stash.Get<Item>().Distinct(new ImageComparer()))
				getImageWithEvents(item);

			foreach (var item in stash.Tabs)
				getImageWithEvents("Tab Icon " + item.i, item.srcC);
		}

		public void GetImages(List<Item> items)
		{
			foreach (var item in items.Distinct(new ImageComparer()))
				getImageWithEvents(item);
		}

		private void getImageWithEvents(Item item)
		{
			getImageWithEvents(GetItemName(item), item.IconURL);
		}

		private void getImageWithEvents(string name, string url)
		{
			onImageLoaded(POEEventState.BeforeEvent, name);
			transport.GetImage(url);
			onImageLoaded(POEEventState.AfterEvent, name);
		}

		public Stream GetImage(Item item)
		{
			onImageLoaded(POEEventState.BeforeEvent, GetItemName(item));
			var ret = transport.GetImage(item.IconURL);
			onImageLoaded(POEEventState.AfterEvent, GetItemName(item));

			return ret;
		}

		public IEnumerable<Stream> GetImage(Tab tab)
		{
			onImageLoaded(POEEventState.BeforeEvent, tab.Name);
			yield return transport.GetImage(tab.srcL);
			yield return transport.GetImage(tab.srcC);
			yield return transport.GetImage(tab.srcR);
			onImageLoaded(POEEventState.AfterEvent, tab.Name);
		}

		private static string GetItemName(Item item)
		{
			if (item.Name != string.Empty)
				return item.Name;

			return item.TypeLine;
		}

		public Dictionary<string, decimal> CalculateFreeSpace(Stash stash)
		{
			return stash.CalculateFreeSpace();
		}

		private void onStashLoaded(POEEventState state, int index, int numberOfTabs)
		{
			if (StashLoading != null)
				StashLoading(this, new StashLoadedEventArgs(index, numberOfTabs, state));
		}

		private void onImageLoaded(POEEventState state, string url)
		{
			if (ImageLoading != null)
				ImageLoading(this, new ImageLoadedEventArgs(url, state));
		}

		private void onAuthenticate(POEEventState state, string email)
		{
			if (Authenticating != null)
				Authenticating(this, new AuthenticateEventArgs(email, state));
		}
	}
}
