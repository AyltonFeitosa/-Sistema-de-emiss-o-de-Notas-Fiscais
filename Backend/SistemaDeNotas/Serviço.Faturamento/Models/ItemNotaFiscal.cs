using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Serviço.Faturamento.Models
{
    public class ItemNotaFiscal
    {
        [Key]
        public int Id { get; set; }
        public string ProdutoId { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public int NotaFiscalId { get; set; }

        [ForeignKey("NotaFiscalId")]
        [JsonIgnore] // Adicione isso para evitar erros de serialização
        public NotaFiscal? NotaFiscal { get; set; }
    }
}