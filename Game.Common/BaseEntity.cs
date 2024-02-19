namespace Game.Common
{
    public abstract class BaseEntity
    {
        public DateTimeOffset DataCriacao { get; set; }
        public DateTimeOffset? DataAtualizacao { get; set; }
        public DateTimeOffset? DataExclusao { get; set; }
    }
}