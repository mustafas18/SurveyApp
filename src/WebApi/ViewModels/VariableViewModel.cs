using Core.Entities;
using Core.Enums;
using System.Text.Json.Serialization;

namespace WebApi.ViewModels
{
    public class VariableViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public VariableTypeEnum Type { get; set; }
        public string Label { get; set; }
        public int MaxValue { get; set; }
        [JsonIgnore]
        public List<VariableValueLabel>? Value { get; set; }
        public string? ValuesAsString { get; set; }
        public MessureEnum Messure { get; set; }
        public string SheetId { get; set; }
    }
}
