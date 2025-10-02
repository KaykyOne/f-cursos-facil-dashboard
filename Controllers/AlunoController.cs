using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plataforma_EAD.Data;
using Plataforma_EAD.Models;

namespace Plataforma_EAD.Controllers
{
    public class AlunoController : Controller
    {
        private readonly AppDbContext _context;

        public AlunoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Aluno
        public async Task<IActionResult> Index()
        {
            return View(await _context.Alunos.ToListAsync());
        }

        // GET: Aluno/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var aluno = await _context.Alunos.FirstOrDefaultAsync(m => m.Id == id);
            if (aluno == null) return NotFound();

            return View(aluno);
        }

        // GET: Aluno/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aluno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Telefone")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aluno);
                await _context.SaveChangesAsync();

                // Toast
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Aluno criado com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            TempData["ToastType"] = "error";
            TempData["ToastMessage"] = "Erro ao criar aluno!";
            return View(aluno);
        }

        // GET: Aluno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return NotFound();

            return View(aluno);
        }

        // POST: Aluno/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Telefone")] Aluno aluno)
        {
            if (id != aluno.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();

                    // Toast
                    TempData["ToastType"] = "success";
                    TempData["ToastMessage"] = "Aluno atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.Id))
                    {
                        TempData["ToastType"] = "error";
                        TempData["ToastMessage"] = "Aluno não encontrado!";
                        return NotFound();
                    }
                    else
                    {
                        TempData["ToastType"] = "error";
                        TempData["ToastMessage"] = "Erro ao atualizar aluno!";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            TempData["ToastType"] = "error";
            TempData["ToastMessage"] = "Erro nos dados fornecidos!";
            return View(aluno);
        }

        // GET: Aluno/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var aluno = await _context.Alunos.FirstOrDefaultAsync(m => m.Id == id);
            if (aluno == null) return NotFound();

            return View(aluno);
        }

        // POST: Aluno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Aluno deletado com sucesso!";
            }
            else
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Erro ao deletar aluno!";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.Id == id);
        }
    }
}
