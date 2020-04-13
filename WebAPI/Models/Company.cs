using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Exchange { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Ticker { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Isin { get; set; }
        [Url]
        public string Website { get; set; }

    }
}
