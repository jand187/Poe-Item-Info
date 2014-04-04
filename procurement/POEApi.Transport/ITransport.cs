using System.IO;
using System.Security;
using POEApi.Infrastructure.Events;

namespace POEApi.Transport
{
    public interface ITransport
    {
        bool Authenticate(string email, SecureString password, bool useSessionID);
        Stream GetStash(int index, string league, bool refresh);
        Stream GetCharacters();
        Stream GetInventory(string characterName);
        event ThottledEventHandler Throttled;
    }
}
