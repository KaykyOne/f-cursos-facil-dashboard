using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plataforma_EAD.Data;
using Plataforma_EAD.Models;

namespace Plataforma_EAD.Controllers
{
    public class CursoController : Controller
    {
        private readonly AppDbContext _context;

        public CursoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Curso
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cursos.ToListAsync());
        }

        // GET: Curso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos.FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null) return NotFound();

            return View(curso);
        }

        // GET: Curso/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Curso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,PrecoBase,CargaHoraria")] Curso curso)
        {
            if (!ModelState.IsValid)
            {
                // Log de erros no console
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"{key}: {error.ErrorMessage}");
                    }
                }

                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Dados inválidos!";
                return View(curso);
            }

            try
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();

                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Curso criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex?.Message ?? "Erro desconhecido");
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Erro ao criar o curso.";
                return View(curso);
            }
        }


        // GET: Curso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();

            return View(curso);
        }

        // POST: Curso/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,PrecoBase,CargaHoraria")] Curso curso)
        {
            if (id != curso.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();

                    TempData["ToastType"] = "success";
                    TempData["ToastMessage"] = "Curso atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.Id))
                    {
                        TempData["ToastType"] = "error";
                        TempData["ToastMessage"] = "Curso não encontrado!";
                        return NotFound();
                    }
                    else throw;
                }
            }

            TempData["ToastType"] = "error";
            TempData["ToastMessage"] = "Erro ao atualizar o curso.";
            return View(curso);
        }


        // GET: Curso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos.FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null) return NotFound();

            return View(curso);
        }

        // POST: Curso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Curso excluído com sucesso!";
            }
            else
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Erro ao excluir o curso.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}
