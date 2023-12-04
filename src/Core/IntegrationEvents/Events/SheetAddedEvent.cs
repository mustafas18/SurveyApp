using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IntegrationEvents.Events
{
    public class SheetAddedEvent : DomainEventBase
    {
        public string SheetId { get; set; }
        public SheetAddedEvent(string sheetId)
        {
            SheetId = sheetId;
        }
    }
}
