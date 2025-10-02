using System.ComponentModel.DataAnnotations;

namespace Plataforma_EAD.Models
{
    public class Aluno
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string? Email { get; set; }
        public string? Telefone { get; set; }

        // Relacionamento 1:N com Matrícula
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}
