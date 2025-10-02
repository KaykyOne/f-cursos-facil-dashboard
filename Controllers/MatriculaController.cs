using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plataforma_EAD.Data;
using Plataforma_EAD.Models;

namespace Plataforma_EAD.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly AppDbContext _context;

        public MatriculaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Matricula
        public async Task<IActionResult> Index()
        {
            var matriculas = _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso);
            return View(await matriculas.ToListAsync());
        }

        // GET: Matricula/Details
        public async Task<IActionResult> Details(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null) return NotFound();

            return View(matricula);
        }

        // GET: Matricula/Create
        public IActionResult Create()
        {
            ViewData["AlunoId"] = new SelectList(_context.Alunos, "Id", "Nome");
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo");
            return View();
        }

        // POST: Matricula/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlunoId,CursoId,Data,PrecoPago,Status,Progresso,NotaFinal")] Matricula matricula)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Dados inválidos!";
                ViewData["AlunoId"] = new SelectList(_context.Alunos, "Id", "Nome", matricula.AlunoId);
                ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo", matricula.CursoId);
                return View(matricula);
            }

            try
            {
                _context.Add(matricula);
                await _context.SaveChangesAsync();

                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Matrícula criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbEx)
            {
                var msg = dbEx.Message;
                if (dbEx.InnerException != null)
                    msg += " | " + dbEx.InnerException.Message;

                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = msg;

                ViewData["AlunoId"] = new SelectList(_context.Alunos, "Id", "Nome", matricula.AlunoId);
                ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo", matricula.CursoId);
                return View(matricula);
            }
        }

        // GET: Matricula/Edit
        public async Task<IActionResult> Edit(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null) return NotFound();

            ViewData["AlunoId"] = new SelectList(_context.Alunos, "Id", "Nome", matricula.AlunoId);
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo", matricula.CursoId);
            return View(matricula);
        }

        // POST: Matricula/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int alunoId, int cursoId, [Bind("AlunoId,CursoId,Data,PrecoPago,Status,Progresso,NotaFinal")] Matricula matricula)
        {
            if (alunoId != matricula.AlunoId || cursoId != matricula.CursoId)
                return NotFound();

            // Regras de validação
            if (matricula.PrecoPago < 0)
                ModelState.AddModelError("PrecoPago", "Preço pago deve ser ≥ 0");
            if (matricula.Progresso < 0 || matricula.Progresso > 100)
                ModelState.AddModelError("Progresso", "Progresso deve estar entre 0 e 100");
            if (matricula.NotaFinal.HasValue && (matricula.NotaFinal < 0 || matricula.NotaFinal > 10))
                ModelState.AddModelError("NotaFinal", "Nota final deve estar entre 0 e 10");
            if (matricula.Status == "Concluido" && matricula.Progresso != 100)
                ModelState.AddModelError("Status", "Não é possível concluir matrícula com progresso diferente de 100");

            if (!ModelState.IsValid)
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Erro ao atualizar matrícula.";
                ViewData["AlunoId"] = new SelectList(_context.Alunos, "Id", "Nome", matricula.AlunoId);
                ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo", matricula.CursoId);
                return View(matricula);
            }

            try
            {
                _context.Update(matricula);
                await _context.SaveChangesAsync();

                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Matrícula atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatriculaExists(matricula.AlunoId, matricula.CursoId))
                {
                    TempData["ToastType"] = "error";
                    TempData["ToastMessage"] = "Matrícula não encontrada!";
                    return NotFound();
                }
                else throw;
            }
        }

        // GET: Matricula/Delete
        public async Task<IActionResult> Delete(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null) return NotFound();

            return View(matricula);
        }

        // POST: Matricula/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas.FindAsync(alunoId, cursoId);
            if (matricula != null)
            {
                _context.Matriculas.Remove(matricula);
                await _context.SaveChangesAsync();
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Matrícula excluída com sucesso!";
            }
            else
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Matrícula não encontrada!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MatriculaExists(int alunoId, int cursoId)
        {
            return _context.Matriculas.Any(e => e.AlunoId == alunoId && e.CursoId == cursoId);
        }
    }
}
