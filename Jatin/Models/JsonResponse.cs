namespace Jatin.Models
{
    public class JsonResponse
    {
        public string msg { get; set; }
        public List<ReminderTaskView>? Tasks { get; set; }
    }

    public class AddJsonResponse
    {
        public string msg { get; set; }
    }

    public class PhoneBookJson
    {
        public string status { get; set; }
        public string msg { get; set; }

    }

    public class PhoneBookContactJson
    {
        public string status { get; set;}
        public List<PhoneBookContact> contacts { get; set; }


    }
}
