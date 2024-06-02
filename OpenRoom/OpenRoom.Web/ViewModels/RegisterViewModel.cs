using System.ComponentModel.DataAnnotations;

namespace OpenRoom.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "電子郵件不能為空")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        public string Email { get; set; }

        [Required(ErrorMessage = "密碼不能為空")]
        [StringLength(100, ErrorMessage = "至少一個大寫字母、一個小寫字母和一個數字，最少8個字符", MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "確認密碼不能為空")]
        [Compare("Password", ErrorMessage = "確認密碼與密碼不匹配")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "姓氏不能為空")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "名字不能為空")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "室內電話號碼不能為空")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "請輸入有效的室內電話號碼")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "手機號碼不能為空")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "請輸入有效的手機號碼")]
        public string Mobile { get; set; }
    }
}
