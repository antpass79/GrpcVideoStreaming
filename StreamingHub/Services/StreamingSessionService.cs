using StreamingHub.Providers;
using Grpc.Core;
using Streaming;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StreamingHub.Services
{
	public class StreamingSessionService : IStreamingSessionService
	{
		private readonly IStreamingSessionProvider _streamingSessionProvider;
		
		public StreamingSessionService(IStreamingSessionProvider streamingSessionProvider)
		{
			_streamingSessionProvider = streamingSessionProvider;
		}
		
		public async Task BroadcastFrameAsync(Frame frame)
		{
			var session = _streamingSessionProvider.GetSessionById(frame.SessionId);
			foreach (var actor in session.ActorsInSession)
			{
				if (actor.Stream == null)
					continue;

				await actor.Stream.WriteAsync(frame);
			}
		}

		public Task<int> AddActorToSessionAsync(Actor actor)
		{
			var session = _streamingSessionProvider.GetFreeSession();
			session.ActorsInSession.Add(new Models.Actor
			{
				Name = actor.Name,
				Id = Guid.Parse(actor.Id)
			});
			return Task.FromResult(session.Id);
		}

		public void ConnectActorToSession(int sessionId, Guid actorId, IAsyncStreamWriter<Frame> responseStream)
		{
			_streamingSessionProvider.GetSessionById(sessionId).ActorsInSession.FirstOrDefault(actor => actor.Id == actorId).Stream = responseStream;
		}
		
		public void DisconnectActor(int sessionId, Guid actorId)
		{
			var session = _streamingSessionProvider.GetSessionById(sessionId);
			session.ActorsInSession.Remove(session.ActorsInSession.FirstOrDefault(actor => actor.Id == actorId));
		}
	}
}