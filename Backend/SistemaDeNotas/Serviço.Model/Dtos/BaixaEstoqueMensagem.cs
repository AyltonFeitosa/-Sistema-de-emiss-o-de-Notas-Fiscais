using System;
using System.Collections.Generic;
using System.Text;

namespace Serviço.Model.Dtos
{
    public class BaixaEstoqueMensagem
    {
        public int NotaFiscalId { get; set; }
        public List<ItemBaixaDto> Itens { get; set; } = new();
    }
}
