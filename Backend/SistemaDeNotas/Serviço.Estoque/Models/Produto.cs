using System.ComponentModel.DataAnnotations;

namespace Serviço.Estoque.Models
{
    public class Produto
    {
        [Key]
        public string UniqueId { get; set; } = Guid.NewGuid().ToString();

        public string Description
        {
            get; set;
        } = string.Empty;

        public int Balance 
        { 
            get; set; 
        }

    }
}
