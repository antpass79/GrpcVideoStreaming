using System;
using System.IO;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Streaming;

namespace StreamingHub.Services
{
	public class StreamingService : Streaming.StreamingService.StreamingServiceBase
	{
		private readonly ILogger<StreamingService> _logger;
		private readonly IStreamingSessionService _streamingServiceService;


		public StreamingService(ILogger<StreamingService> logger, IStreamingSessionService streamingSessionService)
		{
			_logger = logger;
			_streamingServiceService = streamingSessionService;
		}

		async public override Task<JoinToSessionReply> JoinToSession(JoinToSessionRequest request, ServerCallContext context)
		{
			return new JoinToSessionReply
			{
				SessionId = await _streamingServiceService.AddActorToSessionAsync(request.Actor)
			};
		}

		async public override Task<Frame> StreamFromClient(IAsyncStreamReader<Frame> requestStream, ServerCallContext context)
		{
			if (!await requestStream.MoveNext())
			{
				return new Frame();
			}

			try
			{
				while (await requestStream.MoveNext())
				{
					await _streamingServiceService.BroadcastFrameAsync(requestStream.Current);
				}
			}
			catch (IOException)
			{
				_streamingServiceService.DisconnectActor(requestStream.Current.SessionId, Guid.Parse(requestStream.Current.ActorId));
			}

			return new Frame();
		}

		async public override Task StreamFromServer(StreamConnectionRequest request, IServerStreamWriter<Frame> responseStream, ServerCallContext context)
		{
			_streamingServiceService.ConnectActorToSession(request.SessionId, Guid.Parse(request.ActorId), responseStream);

			while (!context.CancellationToken.IsCancellationRequested)
			{
				await Task.Delay(50000);
			}

			await Task.CompletedTask;
		}
	}
}