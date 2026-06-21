using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorDespacho.Models;

public class PedidoDetalleController : Controller
{
    private readonly AplicationDbContext _context;

    public PedidoDetalleController(AplicationDbContext context)
    {
        _context = context;
    }

    // GET: PEDIDODETALLES
    public async Task<IActionResult> Index()
    {
        return View(await _context.PedidosDetalles.ToListAsync());
    }

    // GET: PEDIDODETALLES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var pedidodetalle = await _context.PedidosDetalles.FirstOrDefaultAsync(m => m.Id == id);
        if (pedidodetalle == null) return NotFound();

        return View(pedidodetalle);
    }

    // GET: PEDIDODETALLES/Create
    // Recibe opcionalmente el pedidoId para saber a qué pedido sumarle este producto
    public IActionResult Create(int? pedidoId)
    {
        // Pasamos la lista de todos los productos cargados a la vista
        ViewBag.ProductosLista = _context.Productos.ToList();

        // Mantenemos el ID del pedido actual en la pantalla
        ViewBag.PedidoIdActual = pedidoId;

        return View();
    }

    // POST: PEDIDODETALLES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,PedidoId,ProductoId,Cantidad,Descripcion,CostoUnitario")] PedidoDetalle pedidodetalle)
    {
        // modificacion para comas
        string costoEnviado = Request.Form["CostoUnitario"].ToString();
        if (decimal.TryParse(costoEnviado.Replace(".", ","), out decimal costoLimpio))
        {
            pedidodetalle.CostoUnitario = costoLimpio;
        }
        else if (decimal.TryParse(costoEnviado.Replace(",", "."), out decimal costoLimpioPunto))
        {
            pedidodetalle.CostoUnitario = costoLimpioPunto;
        }

        ModelState.Remove("CostoUnitario");
         //bloque que al sumar 2 productos iguales en un pedido, no duplique renglones sino que agregue cantidades
        if (ModelState.IsValid)
        {
            // buscamos si el producto ya existe en este pedido específico
            var detalleExistente = await _context.PedidosDetalles
                .FirstOrDefaultAsync(d => d.PedidoId == pedidodetalle.PedidoId && d.ProductoId == pedidodetalle.ProductoId);

            if (detalleExistente != null)
            {
                // si ya existe, acumulamos la cantidad
                detalleExistente.Cantidad += pedidodetalle.Cantidad;

                // actualizamos el costo por si cambió en esta carga
                detalleExistente.CostoUnitario = pedidodetalle.CostoUnitario;

                _context.Update(detalleExistente);
            }
            else
            {
                // si no estaba en el pedido, lo agregamos normalmente
                _context.Add(pedidodetalle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Pedido", new { id = pedidodetalle.PedidoId });
        }

        // Si algo falla, recargamos la lista de productos para no romper la pantalla
        ViewBag.ProductosLista = _context.Productos.ToList();
        return View(pedidodetalle);
    }

    // GET: PEDIDODETALLES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var pedidodetalle = await _context.PedidosDetalles.FindAsync(id);
        if (pedidodetalle == null) return NotFound();

        ViewBag.ProductosLista = _context.Productos.ToList();
        return View(pedidodetalle);
    }

    // POST: PEDIDODETALLES/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,PedidoId,ProductoId,Cantidad,Descripcion,CostoUnitario")] PedidoDetalle pedidodetalle)
    {
        if (id != pedidodetalle.Id) return NotFound();

        // modificacion para que tome comas 
        string costoEnviado = Request.Form["CostoUnitario"].ToString();
        if (decimal.TryParse(costoEnviado.Replace(".", ","), out decimal costoLimpio))
        {
            pedidodetalle.CostoUnitario = costoLimpio;
        }
        else if (decimal.TryParse(costoEnviado.Replace(",", "."), out decimal costoLimpioPunto))
        {
            pedidodetalle.CostoUnitario = costoLimpioPunto;
        }

        ModelState.Remove("CostoUnitario");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pedidodetalle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoDetalleExists(pedidodetalle.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction("Details", "Pedido", new { id = pedidodetalle.PedidoId });
        }

        ViewBag.ProductosLista = _context.Productos.ToList();
        return View(pedidodetalle);
    }

    // GET: PEDIDODETALLES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var pedidodetalle = await _context.PedidosDetalles.FirstOrDefaultAsync(m => m.Id == id);
        if (pedidodetalle == null) return NotFound();

        return View(pedidodetalle);
    }

    // POST: PEDIDODETALLES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var pedidodetalle = await _context.PedidosDetalles.FindAsync(id);
        int? pedidoIdOriginal = pedidodetalle?.PedidoId;

        if (pedidodetalle != null)
        {
            _context.PedidosDetalles.Remove(pedidodetalle);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Pedido", new { id = pedidoIdOriginal });
    }

    private bool PedidoDetalleExists(int? id)
    {
        return _context.PedidosDetalles.Any(e => e.Id == id);
    }
}