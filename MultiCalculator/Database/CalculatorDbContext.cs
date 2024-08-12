using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiCalculator.Database.Models;

namespace MultiCalculator.Database
{
    public class CalculatorDbContext : DbContext
    {
        public virtual DbSet<UserModel> User { get; set; }
        public virtual DbSet<CalculationHistoryModel> CalculationHistory { get; set; }
        public virtual DbSet<OpenAiQuestionsModel> OpenAiQuestions { get; set; }
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { });
        public static string FilePath = "";
        public CalculatorDbContext(DbContextOptions<CalculatorDbContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            FilePath = System.IO.Path.Join(path, "database.mdf");

        }

        public CalculatorDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            FilePath = System.IO.Path.Join(path, "database.mdf");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);

            optionsBuilder.UseSqlite($"Data Source={FilePath}");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
