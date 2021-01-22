using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// Klasa IdLabel ima svoj id i string label koja se koristi za autocomplete
    /// </summary>
    public class IdLabel
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public IdLabel() { }
        public IdLabel(int id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}
