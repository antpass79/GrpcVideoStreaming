using Grpc.Core;
using Streaming;
using System;

namespace StreamingHub.Models
{
	public class Actor
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public IAsyncStreamWriter<Frame> Stream { get; set; }
	}
}