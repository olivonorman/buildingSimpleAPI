
using Microsoft.EntityFrameworkCore;
using simpleAPI.Models;

namespace simpleAPI.Data
{
    public class UserApi: DbContext
    {
        public UserApi(DbContextOptions<UserApi> options) : base(options)
        {   
        }

        public DbSet<User> Users => Set<User>();
    }
}
