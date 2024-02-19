using Game.Inventario.Api.Contracts.ItensBatalha;
using Game.Inventario.Api.Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Game.Inventario.Api.Contracts.ItemInventario
{
    public class ItensInventarioResponse
    {
        public Guid ItemIventId { get; set; }
        public ItemBatalhaResponse? ItemBatalha { get; set; }
    }
}
