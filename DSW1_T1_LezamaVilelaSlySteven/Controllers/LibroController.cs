using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using DSW1_T1_LezamaVilelaSlySteven.Models;

namespace DSW1_T1_LezamaVilelaSlySteven.Controllers;

public class LibroController : Controller
{
    private readonly IConfiguration _config;

    public LibroController(IConfiguration config)
    {
        _config = config;
    }

    IEnumerable<Libro> libros()
    {
        List<Libro> temporal = new List<Libro>();
        using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("BDBIBLIOTECA2026")))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("usp_listar_libros", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                temporal.Add(new Libro()
                {
                    CodLibro = dr.GetString(0),
                    CodCategoria = dr.GetString(1),
                    NombreCategoria = dr.GetString(2),
                    Titulo = dr.GetString(3),
                    ISBN = dr.GetString(4),
                    AnioPublicacion = dr.GetInt32(5),
                    Stock = dr.GetInt32(6),
                    Estado = dr.GetByte(7)
                });
            }
            dr.Close();
        }
        return temporal;
    }

    IEnumerable<CategoriaLibro> categorias()
    {
        List<CategoriaLibro> temporal = new List<CategoriaLibro>();
        using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("BDBIBLIOTECA2026")))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("usp_listar_categorias", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                temporal.Add(new CategoriaLibro()
                {
                    CodCategoria = dr.GetString(0),
                    NombreCategoria = dr.GetString(1),
                    Descripcion = dr.IsDBNull(2) ? "" : dr.GetString(2)
                });
            }
            dr.Close();
        }
        return temporal;
    }

    // GET: /Libro
    public IActionResult Index()
    {
        return View(libros());
    }

    // GET: /Libro/Crear
    [HttpGet]
    public IActionResult Crear()
    {
        ViewBag.categorias = new SelectList(categorias(), "CodCategoria", "NombreCategoria");
        return View(new Libro());
    }

    // POST: /Libro/Crear
    [HttpPost]
    public IActionResult Crear(Libro reg)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.categorias = new SelectList(categorias(), "CodCategoria", "NombreCategoria", reg.CodCategoria);
            return View(reg);
        }

        try
        {
            using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("BDBIBLIOTECA2026")))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_registrar_libro", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodLibro", reg.CodLibro.Trim());
                cmd.Parameters.AddWithValue("@CodCategoria", reg.CodCategoria);
                cmd.Parameters.AddWithValue("@Titulo", reg.Titulo.Trim());
                cmd.Parameters.AddWithValue("@ISBN", reg.ISBN.Trim());
                cmd.Parameters.AddWithValue("@AnioPublicacion", reg.AnioPublicacion);
                cmd.Parameters.AddWithValue("@Stock", reg.Stock);
                cmd.Parameters.AddWithValue("@Estado", reg.Estado);
                cmd.ExecuteNonQuery();
            }

            TempData["Mensaje"] = $"Libro '{reg.Titulo}' registrado correctamente.";
            return RedirectToAction("Index");
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627 || ex.Number == 2601)
                ViewBag.Error = "Ya existe un libro con ese Código o ISBN.";
            else
                ViewBag.Error = "Error al registrar el libro: " + ex.Message;

            ViewBag.categorias = new SelectList(categorias(), "CodCategoria", "NombreCategoria", reg.CodCategoria);
            return View(reg);
        }
    }

    // GET: /Libro/Editar/LIB001
    [HttpGet]
    public IActionResult Editar(string id)
    {
        Libro? reg = libros().FirstOrDefault(x => x.CodLibro.Trim() == id.Trim());
        if (reg == null)
        {
            TempData["Error"] = "No se encontró el libro solicitado.";
            return RedirectToAction("Index");
        }

        ViewBag.categorias = new SelectList(categorias(), "CodCategoria", "NombreCategoria", reg.CodCategoria);
        return View(reg);
    }

    // POST: /Libro/Editar
    [HttpPost]
    public IActionResult Editar(Libro reg)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.categorias = new SelectList(categorias(), "CodCategoria", "NombreCategoria", reg.CodCategoria);
            return View(reg);
        }

        try
        {
            using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("BDBIBLIOTECA2026")))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_libro", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodLibro", reg.CodLibro.Trim());
                cmd.Parameters.AddWithValue("@CodCategoria", reg.CodCategoria);
                cmd.Parameters.AddWithValue("@Titulo", reg.Titulo.Trim());
                cmd.Parameters.AddWithValue("@ISBN", reg.ISBN.Trim());
                cmd.Parameters.AddWithValue("@AnioPublicacion", reg.AnioPublicacion);
                cmd.Parameters.AddWithValue("@Stock", reg.Stock);
                cmd.Parameters.AddWithValue("@Estado", reg.Estado);
                cmd.ExecuteNonQuery();
            }

            TempData["Mensaje"] = $"Libro '{reg.Titulo}' actualizado correctamente.";
            return RedirectToAction("Index");
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627 || ex.Number == 2601)
                ViewBag.Error = "El ISBN ingresado ya pertenece a otro libro.";
            else
                ViewBag.Error = "Error al actualizar el libro: " + ex.Message;

            ViewBag.categorias = new SelectList(categorias(), "CodCategoria", "NombreCategoria", reg.CodCategoria);
            return View(reg);
        }
    }

    // POST: /Libro/Eliminar
    [HttpPost]
    public IActionResult Eliminar(string id)
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("BDBIBLIOTECA2026")))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_libro", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodLibro", id.Trim());
                cmd.ExecuteNonQuery();
            }

            TempData["Mensaje"] = $"El libro {id} fue eliminado correctamente.";
        }
        catch (SqlException ex)
        {
            if (ex.Number == 547)
                TempData["Error"] = $"No se puede eliminar el libro {id} porque tiene préstamos relacionados.";
            else
                TempData["Error"] = "No se pudo eliminar el libro: " + ex.Message;
        }

        return RedirectToAction("Index");
    }

    // Inciso c): Método IEnumerable ListarPrestamosAnio que recibe el año
    IEnumerable<PrestamoAnio> ListarPrestamosAnio(int anio)
    {
        List<PrestamoAnio> temporal = new List<PrestamoAnio>();
        using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("BDBIBLIOTECA2026")))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("usp_PrestamosPorAnio", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Anio", anio);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                temporal.Add(new PrestamoAnio()
                {
                    IdPrestamo = dr.GetInt32(0),
                    FechaPrestamo = dr.GetDateTime(1),
                    FechaDevolucion = dr.IsDBNull(2) ? null : dr.GetDateTime(2),
                    ApellidosNombres = dr.GetString(3),
                    Titulo = dr.GetString(4),
                    EstadoPrestamo = dr.GetString(5),
                    Multa = dr.GetDecimal(6)
                });
            }
            dr.Close();
        }
        return temporal;
    }

    // Inciso d): IActionResult de consulta con año por defecto (año actual) y paginación de 10 filas
    public IActionResult PrestamosPorAnio(int? anio = null, int p = 0)
    {
        int anioConsulta = anio ?? DateTime.Now.Year;

        IEnumerable<PrestamoAnio> temporal = ListarPrestamosAnio(anioConsulta);

        int fila = 10;
        int c = temporal.Count();
        int pags = c % fila == 0 ? c / fila : c / fila + 1;

        ViewBag.anio = anioConsulta;
        ViewBag.total = c;
        ViewBag.p = p;
        ViewBag.pags = pags;

        return View(temporal.Skip(fila * p).Take(fila));
    }



}