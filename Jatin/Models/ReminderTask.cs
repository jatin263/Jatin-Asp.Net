using System.ComponentModel.DataAnnotations;

namespace Jatin.Models
{
    public class ReminderTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UId { get; set; }
        public int ActiveStatus { get; set; }

        public DateTime DateAt { get; set; }  
    }

    public class NewReminderTask
    {
        [Required(ErrorMessage ="Name is Requied")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Description is Required")]
        public string Description { get; set; }
        public DateTime DateAt { get; set; }
    }

    public class ReminderTaskView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ActiveStatus { get; set; }

        public string dateAt { get; set; }
    }

    public class UpdateReminder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateAt { get; set; }

    }
}
