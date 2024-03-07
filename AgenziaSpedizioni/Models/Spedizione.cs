using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenziaSpedizioni.Models
{
    public class Spedizione
    {
        [Key]
        [Display(Name = "Codice Identificativo")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [ScaffoldColumn(false)]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Data di spedizione")]
        public DateOnly DataSpedizione { get; set; }
        [Required]
        [Display(Name = "Data di consegna prevista")]
        public DateOnly DataConsegnaPrevista { get; set; }
        [Required]
        public double Peso { get; set; }
        [Required]
        [Display(Name = "Citta destinataria")]
        public string CittaDestinataria { get; set; }
        [Required]
        public string Indirizzo { get; set; }
        [Required]
        [Display(Name = "Costo di spedizione")]
        public double CostoSpedizione { get; set; }

        public virtual ICollection<DettagliSpedizione> DettagliSpedizioni { get; set; }
    }
}
