using System.ComponentModel.DataAnnotations;

namespace DSW1_T1_LezamaVilelaSlySteven.Models;

public class CategoriaLibro
{
    [Display(Name = "Código Categoría")]
    public string CodCategoria { get; set; } = string.Empty;

    [Display(Name = "Categoría")]
    public string NombreCategoria { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }
}