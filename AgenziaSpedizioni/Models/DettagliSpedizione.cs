using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenziaSpedizioni.Models
{
    public class DettagliSpedizione
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [ScaffoldColumn(false)]
        [ForeignKey("Spedizione")]
        public Guid IdSpedizione { get; set; }
        public string Stato { get; set; }
        [Display(Name = "Posizione attuale")]
        public string CittaAttuale { get; set; }
        [Display(Name = "Data aggiornamento")]
        public DateTime dataUpdate { get; set; }

        public virtual Spedizione Spedizione { get; set; }
    }
}
