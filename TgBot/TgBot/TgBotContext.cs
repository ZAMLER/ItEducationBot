using Microsoft.EntityFrameworkCore;
using TgBot.Entities;

namespace TgBot
{
    public class TgBotContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public TgBotContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=SQL8005.site4now.net;Initial Catalog=db_a973a3_zamler001;User Id=db_a973a3_zamler001_admin;Password=thestar3");
            }
        }
    }
    
}
