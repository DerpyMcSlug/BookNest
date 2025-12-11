using BookNest.DataAccess.Data;
using BookNest.DataAccess.Repository;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Product product)
    {
        _db.Products.Update(product);
    }
}
