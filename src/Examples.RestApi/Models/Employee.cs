using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Examples.RestApi.Models;

public class Employee
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [JsonProperty("guid")]
    [JsonPropertyName("guid")]
    public Guid Guid { get; set; }
    [Required]
    [JsonProperty("full_name")]
    [JsonPropertyName("full_name")]
    public string FullName { get; init; }
    [Required]
    [JsonProperty("position")]
    [JsonPropertyName("position")]
    public string Position { get; init; }
    [Range(0, 100)]
    [JsonProperty("age")]
    [JsonPropertyName("age")]
    public int Age { get; init; }

    public Employee()
    {
       
    }
    public Employee(Guid guid)
    {
        Guid = guid;
    }
}