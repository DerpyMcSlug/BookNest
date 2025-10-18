using BookNest.DataAccess.Data;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;

namespace BookNest.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
