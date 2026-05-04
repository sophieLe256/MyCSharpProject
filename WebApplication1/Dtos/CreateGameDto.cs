using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

// public class GameDto
// {
//     public int Id { get; set; }
//     public string Title { get; set; }
//     public string Genre { get; set; }
//     public decimal Price { get; set; }
// }

public record class CreateGameDto
(
    [Required][StringLength(50)] string Name, 
    [Required][StringLength(20)] string Genre, 
    [Range(1.00, 1000.00)] decimal Price, 
    DateOnly ReleaseDate
);