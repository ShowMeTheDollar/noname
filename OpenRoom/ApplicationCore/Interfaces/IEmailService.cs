using ApplicationCore.DTO;

namespace ApplicationCore.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(EmailDto request);

    }
}
