using Grpc.Core;
using Grpc.Net.Client;
using Streaming;

namespace StreamingCore.Actors
{
	public class FrameSender
	{
		public event EventHandler<Frame>? FrameGenerated;
		public event EventHandler<Actor>? ActorConnected;

        async public Task Run(GrpcClientOptions clientOptions = null!)
        {
            var actor = Helper.NewActor();

            var client = BuildClient(clientOptions);
            var joinToSessionReply = await client.JoinToSessionAsync(new JoinToSessionRequest
            {
                Actor = actor
            });

            using (var streaming = client.StreamFromClient(new Metadata { new Metadata.Entry("ActorName", actor.Name) }))
            {
                await streaming.RequestStream.WriteAsync(new Frame
                {
                    ActorId = actor.Id,
					ActorName = actor.Name,
					SessionId = joinToSessionReply.SessionId
                });

                ActorConnected?.Invoke(this, actor);

                string archivePath = @"C:\WORK\Archive\";

                var data = Directory
                    .GetFiles(archivePath, "*.jpg", SearchOption.AllDirectories)
                    .Select(file => File.ReadAllBytes(file))
                    .ToList();

                int n = 0;

                while (true)
                {
                    var frame = new Frame
                    {
                        Id = Guid.NewGuid().ToString(),
                        ActorId = actor.Id,
                        ActorName = actor.Name,
                        SessionId = joinToSessionReply.SessionId,
                        Content = Google.Protobuf.ByteString.CopyFrom(data[n]),
                        GeneratedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
                    };

                    FrameGenerated?.Invoke(this, frame);

					await streaming.RequestStream.WriteAsync(frame);

                    await Task.Delay(50);

                    if (++n >= data.Count)
                        n = 0;
                }
                await streaming.RequestStream.CompleteAsync();
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
