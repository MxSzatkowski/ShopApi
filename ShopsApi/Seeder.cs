using ShopsApi.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ShopsApi
{
    public class Seeder
    {
        private readonly ShopDbContext _context;

        public Seeder(ShopDbContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if(_context.Database.CanConnect())
            {
                if(!_context.Roles.Any())
                {
                    var roles = GetRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Owner"
                },
                new Role
                {
                    Name = "Admin"
                },
            };
            return roles;
        }
    }
}
