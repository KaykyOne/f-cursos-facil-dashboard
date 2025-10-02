using System.ComponentModel.DataAnnotations;

namespace Plataforma_EAD.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [DataType(DataType.Currency)]
        public decimal PrecoBase { get; set; }

        public int CargaHoraria { get; set; }

        // Relacionamento 1:N com Matrícula
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}
