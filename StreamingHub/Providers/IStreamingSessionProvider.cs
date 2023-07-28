using StreamingHub.Models;

namespace StreamingHub.Providers
{
	public interface IStreamingSessionProvider
	{
		Session GetFreeSession();
		Session GetSessionById(int sessionId);
		Session AddSession();
	}
}