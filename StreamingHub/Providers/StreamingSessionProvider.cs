using System.Collections.Generic;
using System.Linq;
using StreamingHub.Models;

namespace StreamingHub.Providers
{
	public class StreamingSessionProvider : IStreamingSessionProvider
	{
		private static readonly Dictionary<int, Session> Sessions = new Dictionary<int, Session>
		(
			new KeyValuePair<int, Session>[]
			{
				new KeyValuePair<int, Session>(1, new Session(1)),
				new KeyValuePair<int, Session>(2, new Session(2)),
				new KeyValuePair<int, Session>(3, new Session(3))
			});
		
		public Session GetFreeSession()
		{
			return Sessions.Values.FirstOrDefault(c => c.ActorsInSession.Count < 10);
		}
		
		public Session GetSessionById(int sessionId)
		{
			return Sessions[sessionId];
		}

		public Session AddSession()
		{
			var newSession = new Session(Sessions.Count + 1);
			Sessions.Add(newSession.Id, newSession);
			return newSession;
		}
	}
}