using System.ComponentModel.DataAnnotations;

namespace GestorDespacho.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = "Operador";
    }
}