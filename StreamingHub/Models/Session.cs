using System.Collections.Generic;

namespace StreamingHub.Models
{
	public class Session
	{
		public int Id { get; }
		public List<Actor> ActorsInSession { get; }
		public Session(int id)
		{
			Id = id;
			ActorsInSession = new List<Actor>();
		}
	}
}