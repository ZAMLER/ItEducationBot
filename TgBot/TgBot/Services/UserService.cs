

using Microsoft.EntityFrameworkCore;
using TgBot.Entities;

namespace TgBot.Services
{
    public class UserService
    {
        public async Task<bool> CheckUserInDB(long id)
        {
            using (var context = new TgBotContext())
            {
                return await context.Users.AnyAsync(item => item.Id == id);
            }
        }
        public async Task CreateUser(User user)
        {
            using (var context = new TgBotContext())
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }

    }
}
