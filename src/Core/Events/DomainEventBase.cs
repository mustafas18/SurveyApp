﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Events
{
    public abstract class DomainEventBase : INotification
    {
        [JsonInclude]
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }

}
