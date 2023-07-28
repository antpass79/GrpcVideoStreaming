using StreamingCore.Actors;
using System;
using System.Threading.Tasks;

namespace StreamingSender
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var frameSender = new FrameSender();

			frameSender.FrameGenerated += (sender, frame) =>
			{
				Console.WriteLine($"Frame generated at {frame.GeneratedAt.ToDateTime().ToLongTimeString()} with Id {frame.Id}");
			};

			frameSender.ActorConnected += (sender, actor) =>
			{
				Console.WriteLine($"{actor.Name} connected to the session");
			};

			await frameSender.Run();
		}
	}
}