using Banana.Services.EmailAPI.Data;
using Banana.Services.EmailAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Banana.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dboptions;

        public EmailService(DbContextOptions<AppDbContext> dboptions)
        {
            _dboptions = dboptions;
        }

        public async Task EmailCartAndLog(CartDto cart)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Email Cart Requested ");
            message.AppendLine("<br/>Total " + cart.CartHeader.CartTotal);
            message.Append("<br/>");
            message.AppendLine("<ul>");
            foreach(var item in cart.CartDetailsList)
            {
                message.Append("<li>");
                message.Append(item.ProductDto.Name + " x " + item.Count);
                message.Append("</li>");

            }
            message.Append("</ul>");

            await EmailAndLog(message.ToString(), cart.CartHeader.Email);
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("User Registraton Successful. <br/> Email : " + email);
            await EmailAndLog(message.ToString(),"lukmanadam43@gmail.com"); 
        }

        private async Task<bool> EmailAndLog(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    Message = message,
                    EmailSent = DateTime.Now
                };

                await using var _db = new AppDbContext(_dboptions);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
    }
}
