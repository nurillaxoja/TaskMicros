using Microsoft.EntityFrameworkCore;
using TaskMIcros.Models;

namespace TaskMIcros.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

         public DbSet<User>  Users { get; set; }

         public DbSet<Income>  Incomes{ get; set; }

        public DbSet<Expenses> Expenses { get; set; }

    }
}
