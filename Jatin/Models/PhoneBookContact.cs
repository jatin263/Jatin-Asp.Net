using System.ComponentModel.DataAnnotations;

namespace Jatin.Models
{
    public class PhoneBookContact
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }

    public class PhoneBookContactAdd
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string uId { get; set; }
    }
    public class PhoneBookContactUpdate 
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string uId { get; set; }
    }
    public class PhoneBookContactDelete
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string uId { get; set; }
    }
}
