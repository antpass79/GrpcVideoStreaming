using System;
using System.Threading.Tasks;
using Grpc.Core;
using Streaming;

namespace StreamingHub.Services
{
	public interface IStreamingSessionService
	{
		Task BroadcastFrameAsync(Frame frame);
		Task<int> AddActorToSessionAsync(Actor actor);
		void ConnectActorToSession(int sessionId, Guid actorId, IAsyncStreamWriter<Frame> asyncStreamWriter);
		void DisconnectActor(int sessionId, Guid actorId);
	}
}