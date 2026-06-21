using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorDespacho.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente? Cliente { get; set; }

        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoTotal { get; set; }

        public int TotalProductos { get; set; }

        public bool Confirmado { get; set; } = false;

        public virtual ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    }
}