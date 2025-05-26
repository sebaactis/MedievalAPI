using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedievalGame.Infraestructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<User> AddAsync(User user)
        {
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (userRole == null)
            {
                throw new Exception("Role user not found");
            }

            user.UserRoles.Add(new UserRole
            {
                RoleId = userRole.Id,
                UserId = user.Id
            });

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteAsync(Guid id)
        {
            var userFind = await context.Users.FindAsync(id);

            if (userFind != null)
            {
                context.Users.Remove(userFind);
                await context.SaveChangesAsync();
                return userFind;
            }

            return null;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Username == username) ?? null;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> UpdateAsync(User user)
        {
            var userFind = await context.Users.FindAsync(user.Id);

            if (userFind != null)
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
                return userFind;
            }

            return null;
        }

        public IEnumerable<string> GetRolesAsync(Guid userId)
        {
            return context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.UserRoles.Select(ur => ur.Role.Name))
                .ToList();
        }
    }
}
