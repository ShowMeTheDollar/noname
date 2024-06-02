using System.ComponentModel.DataAnnotations;

namespace OpenRoom.Web.ViewModels
{
    public class StandardLoginViewModel
    {
        [Required(ErrorMessage = "請輸入電子郵件。")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址。")]
        public string Email { get; set; }

        [Required(ErrorMessage = "請輸入密碼。")]
		[StringLength(100, ErrorMessage = "至少一個大寫字母、一個小寫字母和一個數字，最少8個字符", MinimumLength = 8)]
		public string Password { get; set; }
    }
}
