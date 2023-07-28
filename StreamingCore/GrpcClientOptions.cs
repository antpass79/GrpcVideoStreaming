using Grpc.Net.Client;

namespace StreamingCore
{
	public class GrpcClientOptions
	{
		public string? Url { get; init; }
		public GrpcChannelOptions? ChannelOptions { get; init; }
	}
}
