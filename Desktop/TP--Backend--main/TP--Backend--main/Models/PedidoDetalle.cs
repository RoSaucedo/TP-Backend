using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorDespacho.Models
{
    public class PedidoDetalle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public virtual Pedido? Pedido { get; set; }

        [Required]
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public virtual Producto? Producto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoUnitario { get; set; }

        //esta es la columna que calcula el subtotal 

        [NotMapped] //esto para que sql lo ignore porque es un monto calculado  
        public decimal Subtotal
        {
            get { return Cantidad * CostoUnitario; }
        }
    }
}