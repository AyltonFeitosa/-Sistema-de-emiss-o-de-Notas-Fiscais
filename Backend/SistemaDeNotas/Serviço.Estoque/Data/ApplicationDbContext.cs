using Microsoft.EntityFrameworkCore;
using Serviço.Estoque.Models;

namespace Serviço.Estoque.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Produto> Produtos { get; set; }
    }
}
