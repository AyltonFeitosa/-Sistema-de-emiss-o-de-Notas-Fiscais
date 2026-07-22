using Microsoft.AspNetCore.Mvc;
using Serviço.Faturamento.Data;
using Serviço.Faturamento.Enum;
using Serviço.Faturamento.Messaging;
using Serviço.Faturamento.Models;

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

            string test = "olá";

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
    }
}
