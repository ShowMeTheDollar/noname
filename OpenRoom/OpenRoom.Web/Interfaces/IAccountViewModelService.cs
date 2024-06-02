
namespace OpenRoom.Web.Interfaces
{
    public interface IAccountViewModelService
    {
        AccountDataViewModel GetAccountCard();
        Task<PersonalViewModel> GetPersonalData(int? userId);
        Task<UserProfileViewModel> GetUserProfileViewModel(int userId);
    }
}
