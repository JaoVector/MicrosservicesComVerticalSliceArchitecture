using Game.Common;
using Game.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace Game.Catalogo.Api.Entities
{
    public class ItemBatalha : BaseEntity
    {
        [Key]
        public Guid ItemId { get; set; }
        [Required]
        [StringLength(65)]
        public string? Nome { get; set; }
        [Required]
        [MaxLength]
        public string? Descricao { get; set; }
        [Required]
        public int Ataque { get; set; }
        [Required]
        public int Defesa { get; set; }
        [Required]
        public ItemCategoriaEnum ItemCategoria { get; set; }
    }
}
