using Core.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Notice that the only thing that the AddDomainEvent method is doing is adding an event to the list. 
        /// No event is dispatched yet, and no event handler is invoked yet.
        /// </summary>
        private List<INotification> _domainEvents;
        [NotMapped]
        public List<INotification> DomainEvents => _domainEvents;
        /// <summary>
        ///simply add an event into a list in memory
        /// </summary>
        /// <param name="eventItem"></param>
        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

    }
}
