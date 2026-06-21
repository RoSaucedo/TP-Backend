using System.ComponentModel.DataAnnotations;

namespace GestorDespacho.Models
{
    public class Direccion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La calle y el número son obligatorios.")]
        [StringLength(200)]
        public string CalleNumero { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(100)]
        public string Ciudad { get; set; } = string.Empty;

        public int ClienteId { get; set; }
        public virtual Cliente? Cliente { get; set; }
    }
}