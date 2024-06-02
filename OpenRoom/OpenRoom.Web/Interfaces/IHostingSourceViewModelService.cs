using OpenRoom.Web.ViewModels.Hosting;

namespace OpenRoom.Web.Interfaces
{
    public interface IHostingSourceViewModelService
    {
        SourceViewModel GetSource(int hostingId);

        Task<SourceViewModel> GetSourceAsync(int hostingId);
        ReservationViewModel GetReservations(int hostingId);

        HostDetailsViewModel GetHostDetails(int roomId);
        Task<CalendarViewModel> GetCalendarPrice(int roomId);

        void DeleteRoom(int roomId);
    }
}
