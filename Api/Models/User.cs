namespace Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirsName { get; set; }
        public string Active { get; set; }

    }
}
