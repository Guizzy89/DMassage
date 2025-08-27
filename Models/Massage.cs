using System.ComponentModel.DataAnnotations;

namespace DMassage.Models
{
    public class Massage
    {
        public int Id { get; set; }

        // Название услуги
        [Required]
        public string Name { get; set; }

        // Описание услуги
        [Required]
        public string Description { get; set; }

        // Цена услуги
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
