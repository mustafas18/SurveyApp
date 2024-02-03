using Ardalis.GuardClauses;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class VariableResultDto
    {
        public VariableResultDto(int id, string name, MessureEnum messure, string label)
        {
            Id = id;
            Name = name;
            Label = label;
            Messure = messure;
        }
        public int Id { get;private set; }
         public string Name { get; private set; }
        public string Label { get; private set; }
        public MessureEnum Messure { get; private set; }
        // DDD Patterns comment
        // Using a private collection field, better for DDD Aggregate's encapsulation
        // so _variableAnswers cannot be added from "outside the AggregateRoot" directly to the collection,
        // but only through the method AddAnswers() which includes behavior.
        private readonly List<VariableAnswerDto> _variableAnswers = new List<VariableAnswerDto>();
        
        // Using List<>.AsReadOnly() 
        // This will create a read only wrapper around the private list so is protected against "external updates".
        // It's much cheaper than .ToList() because it will not have to copy all items in a new collection. (Just one heap alloc for the wrapper instance)
        public IReadOnlyCollection<VariableAnswerDto?> Answers => _variableAnswers.AsReadOnly();


        public void AddAnswer(VariableAnswerDto answer)
        {
            Guard.Against.Null(answer);
            _variableAnswers.Add(answer);
        }
        public void AddAnswerList(List<VariableAnswerDto> answers)
        {
            Guard.Against.Null(answers);
            _variableAnswers.AddRange(answers);
        }
    }     
}
