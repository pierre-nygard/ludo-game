using GameEngine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;


namespace TheLudoGame.Context
{
    public class LudoContext : DbContext
    {
        public DbSet<Game> Game { get; set; }
        public DbSet<Board> Board { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Piece> Piece { get; set; }

        public LudoContext(DbContextOptions<LudoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var playerTypeConverter = new EnumToNumberConverter<PlayerType, int>();

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.PlayerType)
                    .HasConversion(playerTypeConverter)
                    .HasDefaultValueSql("((0))");

            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
