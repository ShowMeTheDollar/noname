namespace OpenRoom.Web.Interfaces
{
    public interface IRoomsViewModelService
    {
        Task<RoomDetailViewModel> GetRoomDetailsViewModel(int roomId);
    }
}