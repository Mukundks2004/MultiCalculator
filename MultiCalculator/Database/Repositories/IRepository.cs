using System.Linq.Expressions;

namespace MultiCalculator.Database.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id); // Get entity by ID
        IEnumerable<T> GetAll(); // Get all entities
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate); // Find entities by predicate
        void Add(T entity); // Add entity
        void Update(T entity); // Update entity
        void Remove(T entity); // Remove entity
        void ClearDatabase(); // Clears the entire database of one type
        void SaveChanges(); // Save changes to database
    }
}
