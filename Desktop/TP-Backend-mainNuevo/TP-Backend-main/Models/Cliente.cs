using System.ComponentModel.DataAnnotations;

namespace GestorDespacho.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{2}-\d{8}-\d{1}$", ErrorMessage = "El formato del CUIT debe ser XX-XXXXXXXX-X")]
        public string CUIT { get; set; } = string.Empty;

        public virtual ICollection<Direccion> Direcciones { get; set; } = new List<Direccion>();
    }
}