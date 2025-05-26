using MedievalGame.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedievalGame.Infraestructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Weapon> Weapons => Set<Weapon>();
        public DbSet<Rarity> Rarities => Set<Rarity>();
        public DbSet<WeaponType> WeaponTypes => Set<WeaponType>();
        public DbSet<CharacterClass> CharacterClasses => Set<CharacterClass>();
        public DbSet<ItemType> ItemTypes => Set<ItemType>();
        public DbSet<CharacterAuditLog> CharacterAuditLogs => Set<CharacterAuditLog>();
        public DbSet<ItemAuditLog> ItemAuditLogs => Set<ItemAuditLog>();
        public DbSet<WeaponAuditLog> WeaponAuditLogs => Set<WeaponAuditLog>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserAuditLog> UserAuditLogs => Set<UserAuditLog>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureCharacterModel(modelBuilder);
            ConfigureCharacterItem(modelBuilder);
            ConfigureWeaponModel(modelBuilder);
            ConfigureItemModel(modelBuilder);
            ConfigureRarityModel(modelBuilder);
            ConfigureWeaponTypeModel(modelBuilder);
            ConfigureCharacterClassModel(modelBuilder);
            ConfiguraItemTypeModel(modelBuilder);
            ConfiguraUserModel(modelBuilder);
            ConfiguraUserRoleModel(modelBuilder);
        }

        private static void ConfiguraUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Password).IsRequired();
                entity.HasMany(u => u.Characters)
                      .WithOne(c => c.User)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
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

                entity.HasOne(e => e.CharacterClass)
                       .WithMany(cc => cc.Characters)
                       .HasForeignKey(e => e.CharacterClassId)
                       .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.User)
                        .WithMany(u => u.Characters)
                        .HasForeignKey(c => c.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.Weapons)
                       .WithMany(w => w.Characters)
                       .UsingEntity<Dictionary<string, object>>(
                           "CharacterWeapon",
                           j => j.HasOne<Weapon>().WithMany().HasForeignKey("WeaponId"),
                           j => j.HasOne<Character>().WithMany().HasForeignKey("CharacterId")
                       );
            });
        }

        private static void ConfigureCharacterItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterItem>(entity =>
            {
                entity.HasKey(ci => new { ci.CharacterId, ci.ItemId });

                entity.Property(ci => ci.Quantity).IsRequired();

                entity.HasOne(ci => ci.Character)
                      .WithMany(c => c.CharacterItems)
                      .HasForeignKey(ci => ci.CharacterId);

                entity.HasOne(ci => ci.Item)
                      .WithMany(i => i.CharacterItems)
                      .HasForeignKey(ci => ci.ItemId);
            });
        }

        private static void ConfigureWeaponModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Weapon>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Name).IsRequired().HasMaxLength(20);
                entity.Property(w => w.AttackPower).IsRequired();
                entity.Property(w => w.Durability).IsRequired();

                entity.HasOne(w => w.Rarity)
                        .WithMany(r => r.Weapons)
                        .HasForeignKey(w => w.RarityId)
                        .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(w => w.WeaponType)
                        .WithMany(wt => wt.Weapons)
                        .HasForeignKey(w => w.WeaponTypeId)
                        .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
                entity.Property(i => i.Value).IsRequired();

                entity.HasOne(i => i.Rarity)
                    .WithMany(r => r.Items)
                    .HasForeignKey(i => i.RarityId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.ItemType)
                    .WithMany(it => it.Items)
                    .HasForeignKey(i => i.ItemTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureRarityModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rarity>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(20);
                entity.Property(r => r.Description).IsRequired().HasMaxLength(100);
            });
        }

        private static void ConfigureWeaponTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeaponType>(entity =>
            {
                entity.HasKey(wt => wt.Id);
                entity.Property(wt => wt.Name).IsRequired().HasMaxLength(30);
                entity.Property(wt => wt.Description).HasMaxLength(200);
            });
        }

        private static void ConfigureCharacterClassModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterClass>(entity =>
            {
                entity.HasKey(cc => cc.Id);
                entity.Property(cc => cc.Name).IsRequired().HasMaxLength(30);
                entity.Property(cc => cc.Description).HasMaxLength(200);
            });
        }

        private static void ConfiguraItemTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemType>(entity =>
            {
                entity.HasKey(it => it.Id);
                entity.Property(it => it.Name).IsRequired().HasMaxLength(30);
                entity.Property(it => it.Description).HasMaxLength(200);
            });
        }

        private static void ConfiguraUserRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        }

        public void SeedData()
        {
            if(!Roles.Any())
            {
                Roles.AddRange(new[]
                {
                    new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "Administrator with full access" },
                    new Role { Id = Guid.NewGuid(), Name = "User", Description = "Regular user with limited access" }
                });
            }

            if (!Rarities.Any())
            {
                Rarities.AddRange(new[]
                {
                    new Rarity { Id = Guid.NewGuid(), Name = "Common", Description = "Basic gear" },
                    new Rarity { Id = Guid.NewGuid(), Name = "Rare", Description = "Somewhat special" },
                    new Rarity { Id = Guid.NewGuid(), Name = "Epic", Description = "Very powerful" },
                    new Rarity { Id = Guid.NewGuid(), Name = "Legendary", Description = "Extremely rare and strong" }
                });
            }

            if (!WeaponTypes.Any())
            {
                WeaponTypes.AddRange(new[]
                        {
                    new WeaponType { Id = Guid.NewGuid(), Name = "Sword", Description = "Standard melee weapon" },
                    new WeaponType { Id = Guid.NewGuid(), Name = "Bow", Description = "Ranged weapon" },
                    new WeaponType { Id = Guid.NewGuid(), Name = "Axe", Description = "Heavy melee weapon" },
                    new WeaponType { Id = Guid.NewGuid(), Name = "Staff", Description = "Magic channeling weapon" }
                });
            }

            if (!CharacterClasses.Any())
            {
                CharacterClasses.AddRange(new[]
                        {
                    new CharacterClass { Id = Guid.NewGuid(), Name = "Warrior", Description = "High defense melee class" },
                    new CharacterClass { Id = Guid.NewGuid(), Name = "Archer", Description = "Ranged physical attacker" },
                    new CharacterClass { Id = Guid.NewGuid(), Name = "Mage", Description = "Magic DPS" },
                    new CharacterClass { Id = Guid.NewGuid(), Name = "Cleric", Description = "Healer and support" }
                });
            }

            if (!ItemTypes.Any())
            {
                ItemTypes.AddRange(new[]
                {
                    new ItemType { Id = Guid.NewGuid(), Name = "Potion" },
                    new ItemType { Id = Guid.NewGuid(), Name = "Elixir" },
                    new ItemType { Id = Guid.NewGuid(), Name = "Scroll" },
                    new ItemType { Id = Guid.NewGuid(), Name = "Key" },
                });
            }

            SaveChanges();
        }
    }
}
