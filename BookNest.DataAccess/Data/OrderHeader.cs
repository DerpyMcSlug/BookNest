using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNest.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
