using System.ComponentModel.DataAnnotations;

namespace Jatin.Models
{
    public class PhoneBookContact
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
