using Game.Common;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Game.Inventario.Api.Entities
{
    public class ItemInventario : BaseEntity
    {
        [Key]
        public Guid ItemIventId { get; set; }
        public Guid PersonagemId { get; set; }

        public Guid ItemId { get; set; }
        public ItemBatalha? ItemBatalha { get; set; }
    }
}
