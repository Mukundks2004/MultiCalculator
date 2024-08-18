using System.Linq.Expressions;

namespace MultiCalculator.Database.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CalculatorDbContext _context; // Database context instance

        public Repository(CalculatorDbContext context)
        {
            _context = context; // Injected database context
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id); // Find entity by ID
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList(); // Retrieve all entities
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList(); // Find entities by predicate
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity); // Add entity to DbSet
            SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity); // Update entity in DbSet
            SaveChanges();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity); // Remove entity from DbSet
            SaveChanges();
        }

        public void ClearDatabase()
        {
            _context.Set<T>().RemoveRange(_context.Set<T>().ToList());
        }

        public void SaveChanges()
        {
            _context.SaveChanges(); // Save changes to the database
        }
    }
}
