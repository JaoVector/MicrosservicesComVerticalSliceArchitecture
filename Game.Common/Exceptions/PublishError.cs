﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Common.Exceptions
{
    public class PublishError : Exception
    {
        public PublishError(string message) : base(message) { }
    }
}