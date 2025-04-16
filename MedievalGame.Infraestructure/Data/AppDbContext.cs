using MedievalGame.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedievalGame.Infraestructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Weapon> Weapons => Set<Weapon>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureCharacterModel(modelBuilder);
            ConfigureWeaponModel(modelBuilder);
            ConfigureItemModel(modelBuilder);
        }

        private static void ConfigureCharacterModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Life).IsRequired();
                entity.Property(e => e.Attack).IsRequired();
                entity.Property(e => e.Defense).IsRequired();
                entity.Property(e => e.Level).IsRequired();
                entity.Property(e => e.Class).IsRequired().HasConversion<string>();

                entity.HasMany(c => c.Weapons)
                       .WithOne(w => w.Character)
                       .HasForeignKey(w => w.CharacterId)
                       .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.Items)
                  .WithMany(i => i.Characters)
                  .UsingEntity<Dictionary<string, object>>(
                      "CharacterItem",
                      j => j.HasOne<Item>().WithMany().HasForeignKey("ItemId"),
                      j => j.HasOne<Character>().WithMany().HasForeignKey("CharacterId")
                  );
            });
        }

        private static void ConfigureWeaponModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Weapon>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Name).IsRequired().HasMaxLength(20);
                entity.Property(w => w.AttackPower).IsRequired();
                entity.Property(w => w.Type).IsRequired();
                entity.Property(w => w.Durability).IsRequired();
                entity.Property(w => w.Rarity).IsRequired();
            });
        }

        private static void ConfigureItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
                entity.Property(i => i.Value).IsRequired();
                entity.Property(i => i.Type).IsRequired();
                entity.Property(i => i.Rarity).IsRequired();
            });
        }
    }
}
