

using Microsoft.EntityFrameworkCore;

namespace ReservaBook.Infraestructure.Persistence.Contexts
{
    public class ReservaBookContextc : DbContext
    {
        public ReservaBookContextc(DbContextOptions<ReservaBookContextc> opt) : base(opt) { }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

        }




    }
}
