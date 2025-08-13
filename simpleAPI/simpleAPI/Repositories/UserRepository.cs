using Microsoft.EntityFrameworkCore;
using simpleAPI.Data;
using simpleAPI.Models;

namespace simpleAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserApi db;

        public UserRepository(UserApi _db)
        {
            db = _db;
        }
        public async Task<User> CreateAsync(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await db.Users.FindAsync(id);
            if (entity == null) return false;
            db.Users.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return db.Users.AnyAsync(x => x.Email == email && (excludeId.HasValue|| x.Id != excludeId));
        }

        public Task<List<User>> GetAllAsync() => db.Users.AsNoTracking().ToListAsync();

        public Task<User?> GetByIdAsync(int id) => db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

        public async Task<bool> UpdateAsync(User user)
        {
            var exist = await db.Users.AnyAsync(u => u.Id == user.Id);
            if (!exist) return false;
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
