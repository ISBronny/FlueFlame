using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Examples.Domain.Models;

public class Employee
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [JsonPropertyName("guid")]
    public Guid Guid { get; set; }
    
    [Required]
    [JsonPropertyName("full_name")]
    public string FullName { get; init; }
    
    [Required]
    [JsonPropertyName("position")]
    public string Position { get; init; }
    
    [Range(0, 100)]
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