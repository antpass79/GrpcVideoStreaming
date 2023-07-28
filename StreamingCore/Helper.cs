using Streaming;

namespace StreamingCore
{
	internal static class Helper
	{
		public static Actor NewActor()
		{
			var id = Guid.NewGuid().ToString();

			return new Actor
			{
				Id = id,
				Name = $"Actor-{id}"
			};
		}
	}
}
