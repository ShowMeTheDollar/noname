namespace OpenRoom.Web.ViewModels
{
#nullable disable

    public class AccountDataViewModel
    {
        public List<AccountCardViewModel> AccountCards { get; set; }
    }
    public class AccountCardViewModel
    {
        public string InnerLink { get; set; }
        public string OnlyShowWeb { get; set; }
        public string WebIconClass { get; set; }
        public string PhoneIconClass { get; set; }
        public string AccountTitle { get; set; }
        public string AccountContent { get; set; }
    }

}
