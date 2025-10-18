using BookNest.Models;

namespace BookNest.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company company);
    }
}
