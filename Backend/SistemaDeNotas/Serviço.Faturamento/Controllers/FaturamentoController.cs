using Microsoft.AspNetCore.Mvc;
using Serviço.Faturamento.Data;
using Serviço.Faturamento.Enum;
using Serviço.Faturamento.Messaging;
using Serviço.Faturamento.Models;
using Serviço.Model.Dtos;
using static System.Net.Mime.MediaTypeNames;

namespace Serviço.Faturamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaturamentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RabbitMqProducer _rabbitMq;
        public FaturamentoController(AppDbContext context, RabbitMqProducer rabbitMq)
        {
            _context = context;
            _rabbitMq = rabbitMq;
        }

        [HttpPost]
        public async Task<ActionResult<NotaFiscal>> SaveAsync(NotaFiscal nota)
        {
            var test = "blablabla";

            await _rabbitMq.PublishMessageAsync(test);

            if (nota == null || nota.Itens.Count() < 1 || nota.Status == StatusEnum.Fechada)
            {
                return BadRequest("ERROR: Falha ao criar nota.");
            }

            await _context.Notas_Fiscais.AddAsync(nota);

            var result = await _context.SaveChangesAsync();
            if(result > 0)
            {
                return Ok(nota);
            }
            else
            {
                return BadRequest("ERROR: Falha ao salvar nota.");
            }
        }

        [HttpPost("print/{id}")]
        public async Task<ActionResult<BaixaEstoqueMensagem>> PrintAsync(int id)
        {
            var nota = await _context.Notas_Fiscais.FindAsync(id);
            if(nota == null)            
                return BadRequest();

            if (nota.Status != StatusEnum.Aberta)
                return BadRequest("Status Inválido");

            var idProducts = new List<string>();

            foreach(var idProduct in nota.Itens)
            {
                idProducts.Add(idProduct.ProdutoId);
            }

            var baixarEstoque = new BaixaEstoqueMensagem
            {
                NotaFiscalId = id,
                Itens = new List<ItemBaixaDto>
                {
                    new ItemBaixaDto {ProdutoId = 1, Quantidade = 5},
                    new ItemBaixaDto {ProdutoId = 2, Quantidade = 10}
                }
            };

            await _rabbitMq.PublishMessageAsync(baixarEstoque);

            return Ok();

        }
    }
}
