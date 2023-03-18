

using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TgBot.Entities;
using TgBot.Enums;

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
        public async Task ChangeUserStateById(long id, State state)
        {
            using (var context = new TgBotContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(item => item.Id == id);
                user.State = (int)state;
                await context.SaveChangesAsync();
                
                
            }
        }
    }
}
