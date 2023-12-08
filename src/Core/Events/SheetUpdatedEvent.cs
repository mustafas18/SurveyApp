using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Events
{
    public class SheetUpdatedEvent : INotification
    {
        public string SheetId { get; set; }
        public SheetUpdatedEvent(string sheetId)
        {
            SheetId = sheetId;
        }
    }
}
