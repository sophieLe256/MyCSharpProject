namespace WebApplication1.Dtos;

// public class GameDto
// {
//     public int Id { get; set; }
//     public string Name { get; set; }
//     public string Genre { get; set; }
//     public decimal Price { get; set; }
// }

public record class GameDto(int Id, string Name, string Genre, decimal Price, DateOnly ReleaseDate);


