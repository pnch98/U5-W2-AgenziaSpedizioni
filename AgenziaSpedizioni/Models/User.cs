using System.ComponentModel.DataAnnotations;

namespace AgenziaSpedizioni.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Identificativo { get; set; }
        [Required]
        [Display(Name = "Tipologia account")]
        public string Tipo { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Usertype { get; set; } = "User";

    }
}
