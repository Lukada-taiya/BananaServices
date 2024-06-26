using Banana.Services.EmailAPI.Models.Dto;

namespace Banana.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cart);
        Task RegisterUserEmailAndLog(string email);
    }
}
