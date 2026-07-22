using Serviço.Faturamento.Enum;
using System.ComponentModel.DataAnnotations;

namespace Serviço.Faturamento.Models
{
    public class NotaFiscal
    {
        [Key]
        public int UniqueId
        {
            get; set;
        }
        public StatusEnum Status
        {
            get; set;
        } = StatusEnum.Aberta;
        public List<ItemNotaFiscal> Itens
        {
            get; set;
        } = new();
    }
}
