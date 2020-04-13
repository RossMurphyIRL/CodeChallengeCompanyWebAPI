using System.ComponentModel.DataAnnotations;


namespace WebAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string UserName { get; set; }
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
