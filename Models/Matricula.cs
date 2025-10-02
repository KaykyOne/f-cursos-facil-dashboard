using System.ComponentModel.DataAnnotations;

namespace Plataforma_EAD.Models
{
    public class Matricula
    {
        // Chave composta -> configurada no DbContext
        public int AlunoId { get; set; }
        public Aluno Aluno { get; set; }

        public int CursoId { get; set; }
        public Curso Curso { get; set; }

        public DateTime Data { get; set; }

        public decimal PrecoPago { get; set; }

        [Required]
        public string Status { get; set; } = "Ativo"; // Ativo/Concluido/Cancelado

        [Range(0, 100)]
        public int Progresso { get; set; }

        [Range(0, 10)]
        public decimal? NotaFinal { get; set; }
    }
}
