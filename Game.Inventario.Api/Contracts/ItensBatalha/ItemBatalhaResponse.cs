using Game.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace Game.Inventario.Api.Contracts.ItensBatalha
{
    public class ItemBatalhaResponse
    {
        public string? Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public int Ataque { get; set; }
        public int Defesa { get; set; }
        public ItemCategoriaEnum ItemCategoria { get; set; }
    }
}
