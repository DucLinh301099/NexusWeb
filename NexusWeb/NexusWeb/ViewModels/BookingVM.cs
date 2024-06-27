using NexusWeb.Models;

namespace NexusWeb.ViewModels
{
	public class BookingVM
	{
		public string Name { get; set; } = null!;

		public int? UserId { get; set; }

		public int ConnectionId { get; set; }

		public string Address { get; set; } = null!;

		public string Phone { get; set; } = null!;

		public string Message { get; set; } = null!;
	}
}
