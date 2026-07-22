using Microsoft.EntityFrameworkCore;
using Serviço.Faturamento.Models;

namespace Serviço.Faturamento.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<NotaFiscal> Notas_Fiscais { get; set; }

        public DbSet<ItemNotaFiscal> Items { get; set; }
    }
}
