using Game.Catalogo.Api.Entities;
using Game.Common.Enum;

namespace Game.Catalogo.Api.Contracts.ItemBatalha
{
    public class CriaItemBatalhaRequest
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public int Ataque { get; set; }
        public int Defesa { get; set; }
        public ItemCategoriaEnum ItemCategoria { get; set; }
    }
}
