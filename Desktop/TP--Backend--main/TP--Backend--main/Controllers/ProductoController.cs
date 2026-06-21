using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorDespacho.Models;

public class ProductoController : Controller
{
    private readonly AplicationDbContext _context;

    public ProductoController(AplicationDbContext context)
    {
        _context = context;
    }

    // GET: PRODUCTOS
    public async Task<IActionResult> Index()
    {
        return View(await _context.Productos.ToListAsync());
    }

    // GET: PRODUCTOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // GET: PRODUCTOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PRODUCTOS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Descripcion,Costo")] Producto producto)
    {
        string costoEnviado = Request.Form["Costo"].ToString();
        bool conversionExitosa = false;

        if (decimal.TryParse(costoEnviado.Replace(".", ","), out decimal costoLimpio))
        {
            producto.Costo = costoLimpio;
            conversionExitosa = true;
        }
        else if (decimal.TryParse(costoEnviado.Replace(",", "."), out decimal costoLimpioPunto))
        {
            producto.Costo = costoLimpioPunto;
            conversionExitosa = true;
        }

        ModelState.Remove("Costo");

        if (!conversionExitosa)
        {
            ModelState.AddModelError("Costo", "El costo debe ser un número válido.");
        }
        else if (producto.Costo <= 0)
        {
            ModelState.AddModelError("Costo", "El costo debe ser mayor a 0.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(producto);
    }

    // GET: PRODUCTOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    // POST: PRODUCTOS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Descripcion,Costo")] Producto producto)
    {
        if (id != producto.Id)
        {
            return NotFound();
        }

        string costoEnviado = Request.Form["Costo"].ToString();
        bool conversionExitosa = false;

        if (decimal.TryParse(costoEnviado.Replace(".", ","), out decimal costoLimpio))
        {
            producto.Costo = costoLimpio;
            conversionExitosa = true;
        }
        else if (decimal.TryParse(costoEnviado.Replace(",", "."), out decimal costoLimpioPunto))
        {
            producto.Costo = costoLimpioPunto;
            conversionExitosa = true;
        }

        ModelState.Remove("Costo");

        if (!conversionExitosa)
        {
            ModelState.AddModelError("Costo", "El costo debe ser un número válido.");
        }
        else if (producto.Costo <= 0)
        {
            ModelState.AddModelError("Costo", "El costo debe ser mayor a 0.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.Id))
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
        return View(producto);
    }

    // GET: PRODUCTOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // POST: PRODUCTOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto != null)
        {
            _context.Productos.Remove(producto);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductoExists(int? id)
    {
        return _context.Productos.Any(e => e.Id == id);
    }
}