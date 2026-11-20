using System.ComponentModel.DataAnnotations;

namespace DSW1_T1_LezamaVilelaSlySteven.Models;

public class Libro
{
    [Required(ErrorMessage = "El código de libro es obligatorio")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "El código debe tener 6 caracteres")]
    [Display(Name = "Código")]
    public string CodLibro { get; set; } = string.Empty;

    [Required(ErrorMessage = "Seleccione una categoría")]
    [Display(Name = "Categoría")]
    public string CodCategoria { get; set; } = string.Empty;

    [Display(Name = "Categoría")]
    public string? NombreCategoria { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(120, ErrorMessage = "Máximo 120 caracteres")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ISBN es obligatorio")]
    [StringLength(20, MinimumLength = 10, ErrorMessage = "El ISBN debe tener entre 10 y 20 caracteres")]
    [Display(Name = "ISBN")]
    public string ISBN { get; set; } = string.Empty;

    [Required(ErrorMessage = "El año de publicación es obligatorio")]
    [Range(1450, 2100, ErrorMessage = "El año debe estar entre 1450 y 2100")]
    [Display(Name = "Año de Publicación")]
    public int AnioPublicacion { get; set; }

    [Required(ErrorMessage = "El stock es obligatorio")]
    [Range(0, 99999, ErrorMessage = "El stock no puede ser negativo")]
    [Display(Name = "Stock")]
    public int Stock { get; set; }

    [Display(Name = "Estado")]
    public byte Estado { get; set; } = 1;
}