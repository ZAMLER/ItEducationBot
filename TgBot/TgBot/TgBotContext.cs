using Microsoft.EntityFrameworkCore;
using TgBot.Entities;

namespace TgBot
{
    public class TgBotContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public TgBotContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-TKHDORE;Database=ItEducationBot;TrustServerCertificate=True;Trusted_Connection=True;");
            }
        }
    }
    
}
