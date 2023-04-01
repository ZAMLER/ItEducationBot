﻿

using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Telegram.Bot.Requests;
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
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                user.State = (int)state;
                await context.SaveChangesAsync();
                
                
            }
        }
        public async Task<State> GetUserStateById(long id)
        { 
            using(var context = new TgBotContext())
            {
              var user = await context.Users.FirstOrDefaultAsync(item => item.Id == id);
                if(user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                return (State)user.State;


            }
            
        }
        public async Task<string> GetUserStateDataById(long id)
        {
            using (var context = new TgBotContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(item => item.Id == id);
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                return user.StateData;


            }
        }
        public async Task ChangeUserStateDataById(long id, string stateData)
        {
            using (var context = new TgBotContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(item => item.Id == id);
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                user.StateData = stateData;
                await context.SaveChangesAsync();


            }
        }
    }
}
