using System.ComponentModel.DataAnnotations;

namespace YemekTarifleriUygulamasi.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; } // Kısa açıklama

        public string? ImagePath { get; set; } // Görsel yolu

        [Required]
        public string Ingredients { get; set; } // Malzemeler

        [Required]
        public string Steps { get; set; } // Yapılış adımları

        [Required]
        public string Category { get; set; } // Kategori (örn: Tatlı, Ana Yemek)
    }
}
