using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorDespacho.Models;

public class PedidoController : Controller
{
    private readonly AplicationDbContext _context;

    public PedidoController(AplicationDbContext context)
    {
        _context = context;
    }

    // GET: PEDIDOS
    public async Task<IActionResult> Index()
    {
        return View(await _context.Pedidos.ToListAsync());
    }

    // GET: PEDIDOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        
        var pedido = await _context.Pedidos
            .Include(p => p.Detalles)                
                .ThenInclude(d => d.Producto)        
            .FirstOrDefaultAsync(m => m.Id == id);

        if (pedido == null)
        {
            return NotFound();
        }

        return View(pedido);
    }

    // GET: PEDIDOS/Create
    public IActionResult Create()
    {
        ViewBag.ClientesLista = _context.Clientes.ToList();
        return View();
    }

    // POST: PEDIDOS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Fecha,ClienteId,Cliente,NombreUsuario,MontoTotal,TotalProductos,Confirmado,Detalles")] Pedido pedido)
    {
        pedido.Fecha = DateTime.Now;

        string totalEnviado = Request.Form["MontoTotal"].ToString();

        if (decimal.TryParse(totalEnviado.Replace(".", ","), out decimal totalLimpio))
        {
            pedido.MontoTotal = totalLimpio;
        }
        else if (decimal.TryParse(totalEnviado.Replace(",", "."), out decimal totalLimpioPunto))
        {
            pedido.MontoTotal = totalLimpioPunto;
        }

        ModelState.Remove("MontoTotal");

        if (ModelState.IsValid)
        {
            _context.Add(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(pedido);
    }

    // GET: PEDIDOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
        {
            return NotFound();
        }

        // ESCUDO: Si el pedido ya está confirmado, bloquea el acceso a la edición
        if (pedido.Confirmado)
        {
            return BadRequest("No se puede editar un pedido confirmado.");
        }

        return View(pedido);
    }

    // POST: PEDIDOS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Fecha,ClienteId,Cliente,NombreUsuario,MontoTotal,TotalProductos,Confirmado,Detalles")] Pedido pedido)
    {
        if (id != pedido.Id)
        {
            return NotFound();
        }

        // ESCUDO: Segunda capa de seguridad por si envían el POST modificado por fuera
        var pedidoExistente = await _context.Pedidos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (pedidoExistente != null && pedidoExistente.Confirmado)
        {
            return BadRequest("No se puede modificar un pedido confirmado.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pedido);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(pedido.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(pedido);
    }

    // GET: PEDIDOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pedido = await _context.Pedidos.FirstOrDefaultAsync(m => m.Id == id);
        if (pedido == null)
        {
            return NotFound();
        }

        // ESCUDO: Evita que entren a la pantalla de eliminación si está confirmado
        if (pedido.Confirmado)
        {
            return BadRequest("No se puede eliminar un pedido confirmado.");
        }

        return View(pedido);
    }

    // POST: PEDIDOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
        {
            return NotFound();
        }

        // ESCUDO: Evita el borrado directo por POST
        if (pedido.Confirmado)
        {
            return BadRequest("No se puede eliminar un pedido confirmado.");
        }

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: PEDIDOS/Confirmar/5
    // Este método es el que procesa el botón de Confirmar Pedido
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirmar(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
        {
            return NotFound();
        }

        // Cambiamos el estado para congelar el pedido definitivamente
        pedido.Confirmado = true;

        _context.Update(pedido);
        await _context.SaveChangesAsync();

        // Redirecciona de vuelta a los detalles, donde ya aparecerá todo bloqueado
        return RedirectToAction(nameof(Details), new { id = pedido.Id });
    }

    private bool PedidoExists(int? id)
    {
        return _context.Pedidos.Any(e => e.Id == id);
    }
}