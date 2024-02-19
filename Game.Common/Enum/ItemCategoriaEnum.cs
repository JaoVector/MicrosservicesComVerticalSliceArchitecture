using System.Text.Json.Serialization;

namespace Game.Common.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ItemCategoriaEnum
    {
        Magia = 1,
        Arma = 2,
        Escudo = 3,
        Armadura = 4
    }
}
