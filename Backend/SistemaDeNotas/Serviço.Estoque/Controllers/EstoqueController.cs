using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serviço.Estoque.Data;
using Serviço.Estoque.Models;

namespace Serviço.Estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstoqueController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetById(string id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound(); 
            }
            return Ok(produto); 
        }

        [HttpGet]
        public async Task<ActionResult<List<Produto>>> GetAll()
        {
            var produtos = await _context.Produtos.ToListAsync();
            return Ok(produtos);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> SaveAsync(Produto produto)
        {
            _context.Produtos.Add(produto);
            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = produto.UniqueId }, produto);return
            }

            else
            {
                return BadRequest();
            }

        }

    }
}
