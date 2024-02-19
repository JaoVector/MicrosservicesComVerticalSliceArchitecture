﻿using Game.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Common.Events.ItensBatalha
{
    public class ItemBatalhaCriadoEvent
    {
        public Guid ItemId { get; set; }
        public string? Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public int Ataque { get; set; }
        public int Defesa { get; set; }
        public ItemCategoriaEnum ItemCategoria { get; set; }
    }
}
