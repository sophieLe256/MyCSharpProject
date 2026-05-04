using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

    // public class UpdateGameDto
    // {
    //     public string Name { get; set; }
    //     public string Genre { get; set; }
    //     public decimal Price { get; set; }
    // }

// Vì record + init là immutable, nên khi update:
    // 👉 Tạo object mới và thay thế object cũ trong list
    public record class UpdateGameDto(    
    [Required][StringLength(50)] string Name, 
    [Required][StringLength(20)] string Genre, 
    [Range(1.00, 1000.00)] decimal Price, DateOnly ReleaseDate
);