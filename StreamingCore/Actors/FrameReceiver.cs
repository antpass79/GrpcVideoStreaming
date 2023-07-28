using Grpc.Core;
using Grpc.Net.Client;
using Streaming;

namespace StreamingCore.Actors
{
	public class FrameReceiver
	{
        public event EventHandler<Frame>? FrameArrived;
        public event EventHandler<Actor>? ActorConnected;

        async public Task Run(GrpcClientOptions clientOptions = null!)
        {
			var actor = Helper.NewActor();

			var client = BuildClient(clientOptions);
            var joinToSessionReply = await client.JoinToSessionAsync(new JoinToSessionRequest
            {
                Actor = actor
            });

            using (var streaming = client.StreamFromServer(new StreamConnectionRequest
            {
                ActorId = actor.Id,
                ActorName = actor.Name,
                SessionId = joinToSessionReply.SessionId
            }))
            {
                var response = Task.Run(async () =>
                {
                    while (await streaming.ResponseStream.MoveNext())
                    {
                        FrameArrived?.Invoke(this, streaming.ResponseStream.Current);
                    }
                });

                ActorConnected?.Invoke(this, actor);

                while (response.Status != TaskStatus.Canceled && response.Status != TaskStatus.Faulted)
                {
                    await Task.Delay(50000);
                }
            }
        }

        StreamingService.StreamingServiceClient BuildClient(GrpcClientOptions clientOptions)
        {
            var url = clientOptions == null ? "https://localhost:5001" : clientOptions.Url;
            var options = clientOptions == null ? new GrpcChannelOptions() : clientOptions.ChannelOptions;

            var channel = GrpcChannel.ForAddress(url!, options!);
            var client = new StreamingService.StreamingServiceClient(channel);

            return client;
        }
    }
}
