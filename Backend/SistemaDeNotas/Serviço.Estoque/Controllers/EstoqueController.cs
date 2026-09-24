using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serviço.Estoque.Data;
using Serviço.Model.Dtos;

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

            produto.UniqueId = Guid.NewGuid().ToString();
            _context.Produtos.Add(produto);
            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return Ok(produto);
            }

            else
            {
                return BadRequest();
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllAsync()
        {
            var result = await _context.Produtos.ExecuteDeleteAsync();

            return Ok();
        }

    }
}
